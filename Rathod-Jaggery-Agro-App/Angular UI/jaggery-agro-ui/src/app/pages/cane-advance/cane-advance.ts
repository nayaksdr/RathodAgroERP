import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import Swal from 'sweetalert2';
import { CaneAdvanceService } from '../../services/cane-advance-service';
import { CaneAdvance } from '../../core/models/CaneAdvance';
import { NotificationService } from '../../services/notification';
@Component({
  selector: 'app-cane-advance',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './cane-advance.html',
  styleUrls: ['./cane-advance.css']
})
export class CaneAdvanceComponent implements OnInit {

  // ----- Form & Lists -----
  advances: CaneAdvance[] = [];
  farmers: any[] = [];
  members: any[] = [];
  model: CaneAdvance = this.emptyModel();
  isEdit = false;
  proofFile?: File;

  // ----- Pagination & Search -----
  searchText = '';
  page = 1;
  pageSize = 10;

  constructor(private service: CaneAdvanceService, private notify: NotificationService) {}

  ngOnInit() {
    this.loadFormData();
  }

  /** logic to check if file upload is required */
  isProofRequired(): boolean {
    return this.model.paymentMode === 'Bank' || this.model.paymentMode === 'UPI';
  }

  loadFormData() {
  // We don't show "Success" here to keep the UI clean, 
  // but we handle the error to alert the user if the data is missing.
  this.service.getFormData().subscribe({
    next: (res) => {
      this.farmers = res.farmers;
      this.members = res.members;
      this.loadAdvances();
    },
    error: (err) => {
      console.error('Error fetching form data:', err);
      // Essential notification: The user can't use the form without this data
      this.notify.showError('Failed to load form data. Please ensure the API is running and try again.');
    }
  });
}

  loadAdvances() {
    this.service.getAll().subscribe({
      next: res => {
        this.advances = res.map(a => ({
          ...a,
          farmerName: this.farmers.find(f => f.id === a.farmerId)?.name || 'Unknown',
          memberName: this.members.find(m => m.id === a.memberId)?.name || 'Unknown'
        }));
      },
      error: () => this.notify.showError('Failed to load advances')
    });
  }

  onFileChange(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.proofFile = input.files[0];
    }
  }

  save() {
    // Basic validation
    if (!this.model.farmerId || !this.model.memberId || !this.model.advanceDate || !this.model.amount || !this.model.paymentMode) {
      this.notify.showError('Please fill in all required fields.');
      return;
    }

    // Conditional Validation: Proof is required for Bank/UPI
    if (this.isProofRequired() && !this.proofFile && !this.isEdit) {
      this.notify.showError('Payment proof document is required for Bank/UPI transactions');
      return;
    }

    const fd = new FormData();
    Object.entries(this.model).forEach(([k, v]) => {
      if (v !== null && v !== undefined) fd.append(k, v.toString());
    });

    if (this.proofFile) {
      fd.append('proofFile', this.proofFile);
    }

    const api$ = this.isEdit ? this.service.update(this.model.id!, fd) : this.service.save(fd);

    api$.subscribe({
      next: () => {
        this.notify.showLoading(this.isEdit ? 'Updating your advance...' : 'Adding new advance...');
        this.notify.showSuccess(this.isEdit ? 'Updated successfully' : 'Saved successfully');
        this.reset();
        this.loadAdvances();
      },
      error: () => this.notify.showError('Operation failed')
      
    });
  }

  /** Global Search Filter logic */
  get filteredData() {
    const search = this.searchText.toLowerCase().trim();
    if (!search) return this.advances;

    return this.advances.filter(a => 
      a.farmerName?.toLowerCase().includes(search) ||
      a.memberName?.toLowerCase().includes(search) ||
      a.amount?.toString().includes(search) ||
      a.paymentMode?.toLowerCase().includes(search) ||
      a.remarks?.toLowerCase().includes(search) ||
      a.advanceDate?.includes(search)
    );
  }

  paginatedData(): CaneAdvance[] {
    const data = this.filteredData;
    const start = (this.page - 1) * this.pageSize;
    return data.slice(start, start + this.pageSize);
  }

  totalPages(): number {
    return Math.ceil(this.filteredData.length / this.pageSize);
  }

  edit(row: CaneAdvance) {
    this.model = { ...row };
    this.isEdit = true;
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }
viewProof(fileName: string | undefined) {
  if (!fileName) {
    this.notify.showError('No document uploaded for this record');
    return;
  }

  const url = this.service.getProofUrl(fileName);
  
  // Option 1: Open in New Tab
  window.open(url, '_blank');

}
// --- 3. DELETE (With Confirmation) ---
async delete(id: number) {
  // Wait for user confirmation
  const result = await this.notify.confirm(
    'Are you sure?', 
    'You will not be able to recover this expense record!'
  );

  if (result.isConfirmed) {
    this.notify.showLoading('Deleting...');

    this.service.delete(id).subscribe({
      next: () => {
        this.notify.close();
        this.notify.showSuccess('Deleted successfully.');
        this.loadAdvances();
      },
      error: () => {
        this.notify.close();
        this.notify.showError('Could not delete the item.');
      }
    });
  }
}
 

  reset() {
    this.model = this.emptyModel();
    this.isEdit = false;
    this.proofFile = undefined;
    // Reset file input manually if needed via ViewChild
  }

  private emptyModel(): CaneAdvance {
    return { id: 0, farmerId: 0, memberId: 0, advanceDate: '', amount: 0, paymentMode: '', remarks: '' };
  } 
}