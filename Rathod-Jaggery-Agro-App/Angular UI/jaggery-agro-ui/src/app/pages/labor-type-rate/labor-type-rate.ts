import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { LaborTypeRateService } from '../../services/labor-type-rate.service';
import { LaborTypeRate } from '../../core/models/LaborTypeRate';
import { NotificationService} from '../../services/notification';
@Component({
  selector: 'app-labor-type-rate',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatTableModule,
    MatPaginatorModule,
    MatButtonModule,
    MatInputModule,
    MatSelectModule,
    MatCardModule,
    MatIconModule,
    MatSnackBarModule
  ],
  templateUrl: './labor-type-rate.html',
  styleUrls: ['./labor-type-rate.css']
})
export class LaborTypeRateComponent implements OnInit, AfterViewInit {

  displayedColumns: string[] = [
    'laborTypeName',
    'rate',
    'effectiveFrom',
    'status',
    'actions'
  ];

  dataSource = new MatTableDataSource<LaborTypeRate>();
  laborTypes: any[] = [];

  model: LaborTypeRate = this.getEmptyModel();

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(
    private api: LaborTypeRateService,
    private notify: NotificationService,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.load();
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
  }

  // ================= LOAD DATA =================
  load(): void {
    // Fetch labor types first
    this.api.getLaborTypes().subscribe({
      next: laborTypes => {
        this.laborTypes = laborTypes;

        // Fetch rates and map laborTypeName for display
        this.api.getAll().subscribe({
          next: rates => {
            this.dataSource.data = rates.map(rate => ({
              ...rate,
              laborTypeName: this.laborTypes.find(x => x.id === rate.laborTypeId)?.laborTypeName || ''
            }));
          },
          error: () =>this.notify.showError('Failed to load rates')
        });
      },
      error: () => this.notify.showError('Failed to load labor types')
    });
  }

  // ================= SAVE =================
  save(): void {
    if (!this.model.laborTypeId) {
      this.notify.showError('Please select labor type');
      return;
    }

    if (!this.hasValidRate()) {
      this.notify.showError('Please enter valid rate');
      return;
    }

    if (this.model.id) {
      this.api.update(this.model.id, this.model)
        .subscribe(() => {
          this.notify.showSuccess('Rate updated successfully');
          this.afterSave();
        });
    } else {
      this.api.create(this.model)
        .subscribe(() => {
          this.notify.showSuccess('Rate created successfully');
          this.afterSave();
        });
    }
  }

  hasValidRate(): boolean {
    return (
      (this.model.dailyRate ?? 0) > 0 ||
      (this.model.perTonRate ?? 0) > 0 ||
      (this.model.perProductionRate ?? 0) > 0
    );
  }

  // ================= EDIT =================
  edit(row: LaborTypeRate): void {
    this.model = { ...row };
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  // ================= DELETE =================
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

 
  // ================= FILTER =================
  applyFilter(event: Event): void {
    const value = (event.target as HTMLInputElement).value;
    this.dataSource.filter = value.trim().toLowerCase();
  }

  // ================= RATE DISPLAY =================
  getRate(row: LaborTypeRate): number {
    if (!row) return 0;
    if (row.perTonRate && row.perTonRate > 0) return row.perTonRate;
    if (row.perProductionRate && row.perProductionRate > 0) return row.perProductionRate;
    return row.dailyRate ?? 0;
  }

  // ================= LABOR TYPE CHANGE =================
  onLaborTypeChange(): void {
    const selected = this.laborTypes.find(x => x.id === this.model.laborTypeId);
    if (!selected) return;

    const name = selected.laborTypeName.toLowerCase();

    // Reset all rates first
    this.model.dailyRate = 0;
    this.model.perTonRate = 0;
    this.model.perProductionRate = 0;

    if (name.includes('cane') || name.includes('breaker') || name.includes('ऊस')) {
      this.model.paymentType = 'दर टनावर आधारित (Cane Breaker)';
    } else if (name.includes('jaggery') || name.includes('गुळ')) {
      this.model.paymentType = 'गुळ उत्पादनावर आधारित दर (Jaggery Maker)';
    } else {
      this.model.paymentType = 'दैनिक दर (Regular Labor)';
    }
  }

  // ================= CLEAR =================
  clear(): void {
    this.model = this.getEmptyModel();
  }

  getEmptyModel(): LaborTypeRate {
    return {
      laborTypeId: 0,
      dailyRate: 0,
      perTonRate: 0,
      perProductionRate: 0,
      isActive: true,
      laborTypeName: '',
      paymentType: ''
    };
  }

  afterSave(): void {
    this.clear();
    this.load();
  }

  // ================= SNACKBAR =================
  showMessage(message: string, type: 'success' | 'error' | 'warning') {
    this.snackBar.open(message, 'Close', {
      duration: 3000,
      panelClass: type
    });
  }
}
