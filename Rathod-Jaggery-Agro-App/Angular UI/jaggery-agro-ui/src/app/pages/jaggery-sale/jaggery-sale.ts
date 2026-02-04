import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { JaggerySaleService } from '../../services/jaggery-sale.service';
import { DealerService } from '../../services/dealer.service';
import { LaborService } from '../../services/labor.service';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatMenuModule } from '@angular/material/menu';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { finalize } from 'rxjs';
import { MatNativeDateModule } from '@angular/material/core';
import { NotificationService } from '../../services/notification';
interface PaymentOption {
  id: number;
  name: string;
}
@Component({
  standalone: true,
  selector: 'app-jaggery-sale-dashboard',
  imports: [
    CommonModule,
    FormsModule,
    MatCardModule,
    MatButtonModule,
    MatInputModule,
    MatIconModule,
    MatTableModule,
    MatSelectModule,
    MatFormFieldModule,
    MatMenuModule,
    MatDatepickerModule,
    MatNativeDateModule
  ],
  templateUrl: './jaggery-sale.html',
  styleUrl: './jaggery-sale.css'
})

export class JaggerySaleComponent implements OnInit {
  // 1. Data arrays used by the template
  dealers: any[] = [];
  paidBy: any[] = []; // This matches the *ngFor="let p of paidBy" in HTML
  labors: any[] = []; 
  sales: any[] = [];

  // 2. UI State
 
  editId: number | null = null;
  displayedColumns: string[] = ['dealer', 'total', 'advance', 'remaining', 'actions'];
  selectedFile: File | null = null;

  // 3. The Model (Synchronized with HTML [(ngModel)])
  model: any = this.getEmptyModel();
  filter = { dealerId: null, from: '', to: '' };
  summary = { total: 0, advance: 0, remaining: 0 };

  constructor(
    private service: JaggerySaleService,
    private dealerService: DealerService,
    private notify: NotificationService,
    private laborService: LaborService 
  ) {}

  ngOnInit(): void {
    this.loadDropdowns();   
    this.load();
    
  }
// ================= FILE HANDLING =================

/**
 * Captures the file from the hidden input and stores it for FormData
 */
// Add this variable at the top of your class
imagePreview: string | ArrayBuffer | null = null;

onFileSelected(event: any): void {
  const file: File = event.target.files[0];
  
  if (file) {
    // 1. Validate
    if (!file.type.startsWith('image/')) {
      alert('Please select an image file');
      return;
    }

    this.selectedFile = file;

    // 2. Generate Preview
    const reader = new FileReader();
    reader.onload = () => {
      this.imagePreview = reader.result; // This is the base64 string for the <img> tag
    };
    reader.readAsDataURL(file);
  }
}

// Update afterSave to clear the preview
afterSave() {
  this.model = this.getEmptyModel();
  this.editId = null;
  this.selectedFile = null;
  this.imagePreview = null; // Important: Clear the preview!
}
  // --- DATA LOADING ---

  loadDropdowns() {
    debugger;
  this.service.getDropdownData().subscribe({
    next: (data) => {
      console.log('API Response:', data); // Add this to see if data actually arrived
      this.dealers = data.dealers || [];  // Ensure 'dealers' is lowercase to match C# return Ok(new { dealers = ... })
      this.paidBy = data.members || []; 
      this.labors = data.labors || [];
    },

    error: (err) => {
      this.notify.showError('Failed to load dropdown data');
     
    }
  });
}

 load() {
  

  const dId = this.filter.dealerId === "null" ? undefined : this.filter.dealerId;

  this.service
    .getSalesNew(
      dId || undefined,
      this.filter.from || undefined,
      this.filter.to || undefined
    )
   
    .subscribe({
      next: (res: any[]) => {
        this.sales = (res || []).map(sale => {
          const dealerObj = this.dealers.find(d => d.id === sale.dealerId);
          return {
            ...sale,
            dealerName: sale.dealer?.name || dealerObj?.name || 'N/A'
          };
        });

        this.calculateSummary();
      },
      error: (err) => {
        this.notify.showError('Failed to load jaggery sales data');
        console.error('Fetch Error:', err);
      }
    });
  }
  // --- CALCULATION LOGIC ---

  calculateTotal() {
    this.model.totalAmount = (this.model.quantityInKg || 0) * (this.model.ratePerKg || 0);
    this.updateRemaining();
  }

  updateRemaining() {
    this.model.remainingAmount = (this.model.totalAmount || 0) - (this.model.advancePaid || 0);
  }

  calculateSummary() {
    this.summary.total = this.sales.reduce((acc, curr) => acc + (curr.totalAmount || 0), 0);
    this.summary.advance = this.sales.reduce((acc, curr) => acc + (curr.advancePaid || 0), 0);
    this.summary.remaining = this.sales.reduce((acc, curr) => acc + (curr.remainingAmount || 0), 0);
  }

  // --- ACTIONS ---

  save() {
    // Basic validation based on your HTML requirements
    if (!this.model.dealerId) {
      this.notify.showError('Please select a dealer');
    
      return;
    }

    const fd = new FormData();
    // Match the C# CreateJaggerySaleDto property names exactly
    fd.append('dealerId', this.model.dealerId.toString());
    fd.append('saleDate', this.model.saleDate);
    fd.append('quantityInKg', this.model.quantityInKg.toString());
    fd.append('ratePerKg', this.model.ratePerKg.toString());
    fd.append('advancePaid', this.model.advancePaid.toString());
    fd.append('paidById', (this.model.paidById || '').toString());
    fd.append('laborId', (this.model.laborId || this.labors[0]?.id || '').toString());
    
    if (this.selectedFile) {
      fd.append('proofImage', this.selectedFile);
    }

    const obs = this.editId
      ? this.service.update(this.editId, fd)
      : this.service.create(fd);

    obs.subscribe({
      next: () => {
        this.afterSave();
        this.load();
        this.notify.showSuccess(this.editId ? 'Jaggery sale updated!' : 'Jaggery sale added!'); 
      },
      error: (err) => this.notify.showError(err.error?.message || 'Error saving record')
    });
  }

  edit(row: any) {
    this.editId = row.id;
    // Format date for the input field if necessary
    const formattedDate = row.saleDate ? new Date(row.saleDate).toISOString().split('T')[0] : '';
    this.model = { ...row, saleDate: formattedDate };
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }
// --- 3. DELETE (With Confirmation) ---
async delete(id: number) {
  // Wait for user confirmation
  const result = await this.notify.confirm(
    'Are you sure?', 
    'You will not be able to recover this jaggery sale record!'
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
 


  private getEmptyModel() {
    return {
      dealerId: null,
      laborId: null, 
      paidById: null, // Added to match HTML [(ngModel)]
      saleDate: new Date().toISOString().split('T')[0],
      quantityInKg: 0,
      ratePerKg: 0,
      totalAmount: 0,
      advancePaid: 0,
      remainingAmount: 0
    };
  }
}
