import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { LaborPaymentService, LaborPaymentResponse } from '../../services/labor-payment';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule, MatTableDataSource } from '@angular/material/table'; // ✅ Added DataSource
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { LaborPayment } from '../../core/models/LaborPayment';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { NotificationService } from '../../services/notification';
@Component({
  selector: 'app-labor-payment',
  standalone: true,
  imports: [
    CommonModule, FormsModule, MatCardModule, MatButtonModule,
    MatInputModule, MatIconModule, MatTableModule, MatFormFieldModule,
    MatTooltipModule, MatProgressSpinnerModule,MatDatepickerModule, 
    MatNativeDateModule
  ],
  templateUrl: './labor-payment.html',
  styleUrls: ['./labor-payment.css']
})
export class LaborPaymentComponent implements OnInit {
  // ✅ Columns must strictly match matColumnDef in HTML
  displayedColumns: string[] = ['laborName', 'workAmount', 'rate', 'grossAmount', 'advance', 'netAmount', 'actions'];
  
  // ✅ Using DataSource for better performance and built-in filtering
  dataSource = new MatTableDataSource<LaborPayment>([]);
  
  loading = false;
  summary = { totalGross: 0, totalAdvance: 0, totalNet: 0 };

  filter = {
    laborId: '',
    from: '',
    to: '',
    searchName: '' // ✅ Added for name search
  };

  constructor(private service: LaborPaymentService, private notify: NotificationService) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading = true;
    this.service.getPayments(this.filter).subscribe({
      next: (res: LaborPaymentResponse) => {
        const data = res.data || [];
        this.dataSource.data = data; // ✅ This makes data appear in the table
        this.calculateSummary(data);
      },
      error: (err) => {
        this.notify.showError('Failed to load payments. Please try again.');
        console.error('Error loading payments', err);
        this.dataSource.data = [];
        this.resetSummary();
      },
      complete: () => this.loading = false
    });
  }

  // ✅ Search logic for Labor Name
  applySearch(event: Event): void {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    // Recalculate summary only for the visible (filtered) results
    this.calculateSummary(this.dataSource.filteredData);
  }

  private calculateSummary(data: LaborPayment[]): void {
    this.summary.totalGross = data.reduce((sum, p) => sum + (p.grossAmount || 0), 0);
    this.summary.totalAdvance = data.reduce((sum, p) => sum + (p.advanceAdjusted || 0), 0);
    this.summary.totalNet = this.summary.totalGross - this.summary.totalAdvance;
  }

  clearFilter(): void {
    this.filter = { laborId: '', from: '', to: '', searchName: '' };
    this.dataSource.filter = '';
    this.load();
  }

  private resetSummary(): void {
    this.summary = { totalGross: 0, totalAdvance: 0, totalNet: 0 };
  }

  downloadSlip(p: LaborPayment): void {
    this.service.generateSlip({
      laborId: p.laborId,
      fromDate: p.fromDate,
      toDate: p.toDate
    }).subscribe({
      next: (blob) => {
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = `Slip_${p.laborName}.pdf`;
        link.click();
        this.notify.showSuccess('Slip downloaded successfully');
      },

      error: (err) => {
        this.notify.showError('Failed to download slip. Please try again.');
        console.error('Error downloading slip', err);
      }
    });
  }
}