import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NotificationService } from '../../services/notification';
// Material Imports
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatTooltipModule } from '@angular/material/tooltip';

import { FarmerService } from '../../services/farmer.service';

export interface Farmer {
  id?: number;
  name: string;
  mobile: string;
  address: string;
  isActive: boolean;
}

@Component({
  selector: 'app-farmer-list',
  standalone: true,
  imports: [
    CommonModule, FormsModule, RouterModule,
    MatCardModule, MatButtonModule, MatInputModule, 
    MatIconModule, MatTableModule, MatFormFieldModule, MatTooltipModule
  ],
  templateUrl: './farmer.html',
  styleUrl: './farmer.css'
})
export class FarmerComponent implements OnInit {
  // Use Farmer[] for type safety
  farmers: Farmer[] = []; 
  originalFarmers: Farmer[] = []; 
  searchQuery: string = '';
  
  model: Farmer = { name: '', mobile: '', address: '', isActive: true };
  editId: number | null = null;
  displayedColumns: string[] = ['id', 'name', 'address', 'status', 'actions'];

  constructor(private service: FarmerService,private notify: NotificationService) {}

  ngOnInit() {
    this.load();
  }

  load() {
    this.service.getAll().subscribe({
      next: (res: Farmer[]) => {
        // CRITICAL FIX: Assign data to BOTH variables
        this.originalFarmers = res || [];
        this.applyFilter(); // This will now populate 'this.farmers'
        this.notify.close(); // Close loading notification
      },
     
      
      error: (err) => {
        this.notify.showError('Failed to load farmers');
        console.error('Load failed', err);
      }
    });
  }

  applyFilter() {
    const query = this.searchQuery.toLowerCase().trim();
    
    if (!query) {
      this.farmers = [...this.originalFarmers];
    } else {
      this.farmers = this.originalFarmers.filter(f => 
        f.name?.toLowerCase().includes(query) || 
        f.id?.toString().includes(query) ||
        f.mobile?.includes(query)
      );
    }
  }

  save() {
    const request = this.editId 
      ? this.service.update(this.editId, this.model) 
      : this.service.create(this.model);
this.notify.showSuccess(this.editId ? 'Farmer updated successfully!' : 'Farmer created successfully!');
    request.subscribe({
      next: () => this.afterSave(),
      error: (err) => {
        this.notify.showError('Save operation failed');       
      }
    });
  }

  edit(farmer: Farmer) {
    this.editId = farmer.id ?? null;
    this.model = { ...farmer };
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

// --- 3. DELETE (With Confirmation) ---
async delete(id: number) {
  // Wait for user confirmation
  const result = await this.notify.confirm(
    'Are you sure?', 
    'You will not be able to recover this farmer record!'
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

  afterSave() {
    this.editId = null;
    this.model = { name: '', mobile: '', address: '', isActive: true };
    this.searchQuery = ''; // Reset search to show new data
    this.load();
  }
}