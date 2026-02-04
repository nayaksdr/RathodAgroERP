import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { LaborTypeService } from '../../services/labor-type.service';
import { LaborType } from '../../core/models/labor-type.model';
import { NotificationService } from '../../services/notification';
declare var bootstrap: any;

@Component({
  selector: 'app-labor-type',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatCardModule,
    MatInputModule,
    MatButtonModule,
    MatTableModule,
    MatIconModule
  ],
  templateUrl: './labor-type.html',
  styleUrls: ['./labor-type.css']
})
export class LaborTypeComponent implements OnInit {

  // ================= DATA =================
 laborTypes: LaborType[] = [];
  filteredData: any[] = [];
  paginatedData: any[] = [];

  model: any = {
    laborTypeName: '',
    description: '',
    isActive: true
  };

  // ================= SEARCH =================
  searchText = '';

  // ================= PAGINATION =================
  currentPage = 1;
  pageSize = 5;

  // ================= DELETE =================
  selectedId!: number;

  // ================= TOAST =================
  showToast = false;
  toastMessage = '';

  constructor(private api: LaborTypeService, private notify: NotificationService) {}

  // ================= INIT =================
  ngOnInit(): void {
    this.load();
  }

  // ================= LOAD =================
  load() {
    this.api.getAll().subscribe({
      next: (res) => {
        this.laborTypes = res;
        this.filteredData = [...this.laborTypes];
        this.updatePagination();
        this.notify.close(); // Close loading notification
      },
      
      error: (err) => {
        this.notify.showError('Failed to load labor types. Please try again.');
        console.error('Error loading labor types', err);
      }
    });
  }

  // ================= SAVE =================
  save() {
    if (!this.model.laborTypeName?.trim()) {
      this.notify.showError('Labor Type Name is required');
      return;
    }

    if (this.model.id) {
      this.api.update(this.model.id, this.model)
        .subscribe(() =>this.notify.showSuccess('Updated successfully'));
    } else {
      this.api.create(this.model)
        .subscribe(() => this.notify.showSuccess('Created successfully'));
    }
  }

  afterSave(message: string) {
    this.clear();
    this.load();
    this.showSuccess(message);
  }

  // ================= EDIT =================
  edit(row: any) {
    this.model = { ...row };
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  // ================= DELETE =================
  confirmDelete(row: any) {
    this.selectedId = row.id;
    const modal = new bootstrap.Modal(
      document.getElementById('deleteModal')!
    );
    modal.show();
  }
// --- 3. DELETE (With Confirmation) ---
async deleteConfirmed(id: number) {
  // Wait for user confirmation
  const result = await this.notify.confirm(
    'Are you sure?', 
    'You will not be able to recover this expense record!'
  );

  if (result.isConfirmed) {
    this.notify.showLoading('Deleting...');

    this.api.delete(id).subscribe({
      next: () => {
        this.notify.close();
        this.notify.showSuccess('Deleted successfully.');
        this.load();
      },
      error: () => {
        this.notify.close();
        this.notify.showError('Could not delete the item.');
      }
    });
  }
}
  

  // ================= CLEAR =================
  clear() {
    this.model = {
      laborTypeName: '',
      description: '',
      isActive: true
    };
  }

  // ================= SEARCH =================
  applyFilter() {
    this.filteredData = this.laborTypes.filter(x =>
      x.laborTypeName?.toLowerCase()
        .includes(this.searchText.toLowerCase())
    );

    this.currentPage = 1;
    this.updatePagination();
  }

  // ================= PAGINATION =================
  updatePagination() {
    const start = (this.currentPage - 1) * this.pageSize;
    const end = start + this.pageSize;
    this.paginatedData = this.filteredData.slice(start, end);
  }

  get totalPages(): number {
    return Math.ceil(this.filteredData.length / this.pageSize);
  }

  get totalPagesArray(): number[] {
    return Array(this.totalPages).fill(0).map((x, i) => i + 1);
  }

  nextPage() {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.updatePagination();
    }
  }

  prevPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.updatePagination();
    }
  }

  goToPage(p: number) {
    this.currentPage = p;
    this.updatePagination();
  }

  // ================= TOAST =================
  showSuccess(message: string) {
    this.toastMessage = message;
    this.showToast = true;

    setTimeout(() => {
      this.showToast = false;
    }, 3000);
  }

}
