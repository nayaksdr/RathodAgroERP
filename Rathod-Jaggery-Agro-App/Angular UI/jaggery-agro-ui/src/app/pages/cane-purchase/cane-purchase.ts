import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import Swal from 'sweetalert2';
import { NotificationService } from '../../services/notification';
import { CanePurchase } from '../../core/models/CanePurchase';
import {
  CanePurchaseService,
  CanePurchaseSummary
} from '../../services/cane-purchase.service';

@Component({
  selector: 'app-cane-purchase-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './cane-purchase.html',
  styleUrls: ['./cane-purchase.css']
})
export class CanePurchaseDashboardComponent implements OnInit {
  data: CanePurchase[] = [];         
  filteredData: CanePurchase[] = []; 
  summary: CanePurchaseSummary[] = [];
  farmers: any[] = [];               
  
  // File Upload State
  selectedFile: File | null = null;
  imagePreview: string | null = null;

  searchQuery: string = '';
  model: any = this.emptyModel(); // Use 'any' or update model to include farmerId
  isEdit = false;

  constructor(private service: CanePurchaseService, private notify: NotificationService) {}

  ngOnInit(): void {
    this.loadAll();
    this.loadFarmers();
  }

// GETTERS FOR THE SUMMARY CARDS
  get totalTons(): number {
    return this.data.reduce((a, b) => a + (Number(b.tons) || 0), 0);
  }

  get totalAmount(): number {
    return this.data.reduce((a, b) => a + (Number(b.amount) || 0), 0);
  }
// Calculation Helper
calculateAmount() {
  this.model.amount = (this.model.tons || 0) * (this.model.rate || 0);
}

// File Selection
onFileSelected(event: Event): void {
  const input = event.target as HTMLInputElement;

  if (!input.files || input.files.length === 0) {
    return;
  }

  const file = input.files[0];
  this.selectedFile = file;

  const reader = new FileReader();

  reader.onload = () => {
    const result = reader.result;
    this.imagePreview = typeof result === 'string' ? result : null;
  };

  reader.readAsDataURL(file);
}


  loadAll(): void {
    this.service.getAll().subscribe({
      next: res => {
        this.data = res || [];
        this.applyFilter();
      },
      error: () => this.notify.showError('Failed to load cane purchase records')
    });
    this.service.getSummary().subscribe(res => this.summary = res || []);
  }

  loadFarmers(): void {
    // Using the corrected method name from your service
    this.service.getFarmersList().subscribe(res => this.farmers = res || []);
  }

  

  save(): void {
    // Calculate amount before sending
    this.model.amount = (this.model.tons || 0) * (this.model.rate || 0);

    const api$ = this.isEdit && this.model.id
      ? this.service.update(this.model.id, this.model)
      : this.service.create(this.model);

    api$.subscribe({
      next: (res: any) => {
        const recordId = this.isEdit ? this.model.id : res.id;
        const selectedFarmer = this.farmers.find(f => f.id == this.model.farmerId);
        if (selectedFarmer)
           {
    this.model.farmerName = selectedFarmer.name;
    }
        // If a file is selected, upload it now
        if (this.selectedFile && recordId) {
          this.uploadImage(recordId);
        } else {
          this.notify.showSuccess(this.isEdit ? 'Record Updated' : 'Record Saved');
          
        }
      },
      error: () => this.notify.showError('Operation failed')
    });
  }

  private uploadImage(id: number): void {
    this.service.uploadCaneWeightImage(id, this.selectedFile!).subscribe({
      next: () => this.finalizeSave('Saved with Image'),
      error: () => this.notify.showError('Record saved, but image upload failed')
    });
  }

  private finalizeSave(msg: string): void {
    Swal.fire('Success', msg, 'success');
    this.reset();
    this.loadAll();
  }

  applyFilter(): void {
    const query = this.searchQuery.toLowerCase().trim();
    this.filteredData = this.data.filter(item => 
      item.farmerName?.toLowerCase().includes(query) || 
      item.id?.toString().includes(query)
    );
  }

  edit(row: CanePurchase): void {
    this.isEdit = true;
    this.model = { ...row };
    this.imagePreview = row.caneWeightImagePath ? row.caneWeightImagePath : null;
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }
/* =============================
    DELETE RECORD
============================= */
// --- 3. DELETE (With Confirmation) ---
async delete(id: number) {
  // Wait for user confirmation
  const result = await this.notify.confirm(
    'Are you sure?', 
    'You won\'t be able to revert this purchase record!!'
  );

  if (result.isConfirmed) {
    this.notify.showLoading('Deleting...');

    this.service.delete(id).subscribe({
      next: () => {
        this.notify.close();
        this.notify.showSuccess('Deleted successfully.');
        this.loadAll();
      },
      error: () => {
        this.notify.close();
        this.notify.showError('Could not delete the item.');
      }
    });
  }
}

  reset(): void {
    this.model = this.emptyModel();
    this.isEdit = false;
    this.selectedFile = null;
    this.imagePreview = null;
  }

  private emptyModel() {
    return { farmerId: null, date: '', tons: 0, rate: 0, amount: 0 };
  }
}