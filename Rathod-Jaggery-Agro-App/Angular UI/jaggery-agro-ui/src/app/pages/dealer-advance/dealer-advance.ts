import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule, Router } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select'; // Added for dropdown
import { MatTooltipModule } from '@angular/material/tooltip';
import { DealerAdvanceService, DealerAdvance } from '../../services/dealer-advance.service';
import { Alert } from '../../shared/alert/alert';
import { NotificationService } from '../../services/notification';

@Component({
  standalone: true,
  selector: 'app-dealer-advance-list',
  imports: [
    CommonModule, FormsModule, RouterModule, MatIconModule,
    MatCardModule, MatInputModule, MatFormFieldModule,
    MatButtonModule, MatTooltipModule, MatSelectModule
  ],
  templateUrl: './dealer-advance.html',
  styleUrls: ['./dealer-advance.css']
})
export class DealerAdvanceListComponent implements OnInit {
  // Logic Variables
  selectedImage: string | null = null;
  list: DealerAdvance[] = [];
  dealers: any[] = []; // For the dropdown
  loading = false;
  selectedFile: File | null = null; // For file upload

  // Filters
  filter = { dealerId: '', from: '', to: '' };

  // Pagination
  page = 1;
  pageSize = 5;

  constructor(
    private service: DealerAdvanceService,
    private notify: NotificationService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.load();
    this.loadDealers(); // Load dealers for the dropdown
  }

  // Fetch Dealer List for Dropdown
 loadDealers(): void {
  // Pass an empty object to get all dealers without date/id filters
  this.service.get({}).subscribe({
    next: (res: any) => {
      // Check if the response is a PagedResult (res.data) or a direct array (res)
      this.dealers = res.data ? res.data : res;
      this.notify.showLoading(this.dealers.length + ' dealers loaded for dropdown');
     
    },
    error: (err) => {
      this.notify.showError('Failed to load dealer list');
      console.error('Error fetching dealer list', err);
    }
  });
}

// FIX: This method must exist for (click)="search()" in HTML
  search(): void {
    this.page = 1; // Reset to first page on new search
    this.loading = true;
    this.service.get(this.filter).subscribe({
      next: (res: any) => {
        // Handle both direct array or paged result
        this.list = res.data ? res.data : res;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        this.notify.showError('Failed to load records');
      }
    });
  }
  load(): void {
    this.loading = true;
    this.service.get(this.filter).subscribe({
      next: (res: DealerAdvance[]) => {
        this.list = res || [];
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        this.notify.showError('Failed to load advance payments');
      }
    });
  }

  // File Selection Handler
  onFileSelected(event: any): void {
    this.selectedFile = event.target.files[0];
  }

  /* ===============================
     CREATE / UPDATE WITH FILE
  =============================== */
  save(item: any): void {
    const formData = new FormData();
    formData.append('dealerId', item.dealerId);
    formData.append('amount', item.amount);
    formData.append('paymentMode', item.paymentMode);
    formData.append('paymentDate', item.paymentDate);
    
    if (this.selectedFile) {
      formData.append('proofImage', this.selectedFile);
    }

    const action = item.id ? this.service.update(item.id, formData) : this.service.create(formData);

    action.subscribe({
      next: () => {
        this.notify.close(); // Close any existing loading notification
        this.notify.showSuccess(item.id ? 'Advance updated successfully!' : 'Advance added successfully!');
        this.load();
      },
      error: (err) => {
        this.notify.close(); // Close any existing loading notification
        this.notify.showError('Operation failed: ' + err.message);
      }
    });
  }
edit(id: number): void {
    console.log('Navigating to edit for ID:', id);
    this.router.navigate(['/dealer-advance/edit', id]);
  }
  // Get total for the UI
  get totalAmount(): number {
    return this.list.reduce((sum, item) => sum + (item.amount || 0), 0);
  }

  // Pagination Logic
  get paginatedData(): DealerAdvance[] {
    const start = (this.page - 1) * this.pageSize;
    return this.list.slice(start, start + this.pageSize);
  }

  totalPages(): number { return Math.ceil(this.list.length / this.pageSize) || 1; }
  nextPage(): void { if (this.page < this.totalPages()) this.page++; }
  previousPage(): void { if (this.page > 1) this.page--; }

  // --- 3. DELETE (With Confirmation) ---
async delete(id: number) {
  // Wait for user confirmation
  const result = await this.notify.confirm(
    'Are you sure?', 
    'You will not be able to recover this advance record!'
  );

  if (result.isConfirmed) {
    this.notify.showLoading('Deleting...');

    this.service.delete(id).subscribe({
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

 
}