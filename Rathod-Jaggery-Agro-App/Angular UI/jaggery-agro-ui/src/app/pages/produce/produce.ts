import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { ProduceService } from '../../services/produce';
import { ProduceChartComponent } from '../../shared/charts/produce-chart/produce-chart';
import { NotificationService } from '../../services/notification';
@Component({
  selector: 'app-produce',
  standalone: true,
  imports: [
    CommonModule, FormsModule, MatCardModule, MatButtonModule, 
    MatTableModule, MatInputModule, MatIconModule, MatDialogModule,
    MatDatepickerModule, MatNativeDateModule, ProduceChartComponent
  ],
  templateUrl: './produce.html',
  styleUrl: './produce.css',
})
export class ProduceComponent implements OnInit {
  @ViewChild('produceForm') produceForm!: TemplateRef<any>;

  list: any[] = [];
  totalCost = 0;
  totalBellyCount = 0;
  totalProductionWeight = 0;

  from?: Date;
  to?: Date;
  formData: any = {};
  displayedColumns = ['date', 'batch', 'weight', 'count', 'production', 'quality', 'stock', 'actions'];

  constructor(private api: ProduceService, private dialog: MatDialog,private notify: NotificationService) {}

  ngOnInit() {
    this.load();
  }

  // Auto-calculate logic for the Add/Edit Form
  calculateFormTotals() {
    const weight = Number(this.formData.quantityKg) || 0;
    const count = Number(this.formData.unitPrice) || 0;

    // Logic: Weight * Count = Total Production
    const total = weight * count;
    
    this.formData.totalCostSnapshot = total;
    this.formData.stockKg = total; // Sync stock with production initially
  }

  load() {
    const fromStr = this.from instanceof Date ? this.from.toISOString().split('T')[0] : undefined;
    const toStr = this.to instanceof Date ? this.to.toISOString().split('T')[0] : undefined;

    this.api.getAll(fromStr, toStr).subscribe({
      next: (res: any) => {
        this.list = res.data || [];
        this.calculateTotals();
        this.notify.close(); // Close loading notification
      },
      error: (err) => {
        this.notify.showError('Failed to load produce data. Please try again.');
        console.error('Data load failed:', err);
      }
    });
  }

  calculateTotals() {
    if (!this.list.length) {
      this.totalBellyCount = 0;
      this.totalProductionWeight = 0;
      this.totalCost = 0;
      return;
    }
    this.totalProductionWeight = this.list.reduce((acc, curr) => acc + (Number(curr.stockKg) || 0), 0);
    this.totalBellyCount = this.list.reduce((acc, curr) => acc + (Number(curr.unitPrice) || 0), 0);
    this.totalCost = this.list.reduce((acc, curr) => acc + (Number(curr.totalCost) || 0), 0);
  }

  openForm(item: any = {}) {
    // Clone item or set defaults for new entry
    this.formData = item.id ? { ...item } : {
      producedDate: new Date(),
      qualityGrade: 'A',
      quantityKg: 0,
      unitPrice: 0,
      totalCostSnapshot: 0,
      stockKg: 0
    };

    const dialogRef = this.dialog.open(this.produceForm, { width: '500px' });
    dialogRef.afterClosed().subscribe(result => {
      if (result) this.save(result);
    });
  }

  save(data: any) {
    const request = data.id ? this.api.update(data.id, data) : this.api.create(data);
    request.subscribe({
      next: () => {
        this.load();
        this.notify.showSuccess('Produce data saved successfully');
      },
      error: (err) => {
        this.notify.showError('Error saving produce data: ' + err.message);
      }
    });
  }
async delete(id: number) {
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

  clearFilter() {
    this.from = undefined;
    this.to = undefined;
    this.load();
  }
}