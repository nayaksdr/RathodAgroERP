
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DealerService } from '../../services/dealer.service';
import { Component,OnInit } from '@angular/core';
// Required Material Imports for the "Awesome" UI
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatTooltipModule } from '@angular/material/tooltip';
import { NotificationService } from '../../services/notification';

@Component({
  standalone: true,
  selector: 'app-dealer',
  imports: [
    CommonModule, 
    FormsModule, 
    MatIconModule, 
    MatButtonModule, 
    MatCardModule, 
    MatInputModule, 
    MatFormFieldModule,
    MatTooltipModule
  ],
  templateUrl: './dealer.html',
  styleUrl: './dealer.css'
})
export class DealerComponent implements OnInit {

  dealers: any[] = [];
  totalItems = 0; // Added for the stat card

  // form model
  model: any = {
    name: '',
    mobile: '',
    address: ''
  };

  // ui state
  isEdit = false;
  selectedId: number | null = null;

  // search + pagination
  search = '';
  page = 1;
  totalPages = 0;

  constructor(private service: DealerService, private notify: NotificationService) {}

  ngOnInit() {
    this.load();
  }

  load() {
    // Ensure we are passing the current search term and page
    this.service.getAll(this.search, this.page)
      .subscribe({
        next: (res: any) => {
          // Check if your API returns data inside a 'data' property
          this.dealers = res.data || [];
          this.totalPages = res.totalPages || 0;
          this.totalItems = res.totalItems || this.dealers.length; 
        },   
        
        error: (err) => {
          this.notify.showError('Failed to load dealer list. Please try again.');
          console.error('Error loading dealers:', err);
        }
      });
  }

  save() {
    if (!this.model.name) return alert('Dealer name required');

    const payload = { ...this.model };

    const apiCall = this.isEdit
      ? this.service.update(this.selectedId!, payload)
      : this.service.create(payload);

    apiCall.subscribe({
      next: () => {
        this.reset();
        this.load();
        this.notify.close();
        this.notify.showSuccess(this.isEdit ? 'Dealer updated!' : 'Dealer added!');
      },
      error: (err) => this.notify.showError('Operation failed: ' + err.message)
    });
  }

  edit(item: any) {
    this.isEdit = true;
    this.selectedId = item.id;
    // Deep copy to prevent editing the table row directly before saving
    this.model = { ...item };
    window.scrollTo({ top: 0, behavior: 'smooth' }); // Smooth scroll to form
  }

  reset() {
    this.model = { name: '', phone: '', address: '' };
    this.isEdit = false;
    this.selectedId = null;
  }
  // --- 3. DELETE (With Confirmation) ---
async delete(id: number) {
  // Wait for user confirmation
  const result = await this.notify.confirm(
    'Are you sure?',     
    'You will not be able to recover this dealer record!'
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
 

  next() {
    if (this.page < this.totalPages) {
      this.page++;
      this.load();
    }
  }

  prev() {
    if (this.page > 1) {
      this.page--;
      this.load();
    }
  }
}