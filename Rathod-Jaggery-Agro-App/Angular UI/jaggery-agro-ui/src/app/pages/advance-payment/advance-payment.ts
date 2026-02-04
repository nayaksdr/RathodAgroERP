import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import Swal from 'sweetalert2';
import { AdvancePayment } from '../../core/models/AdvancePayment';
import { AdvancePaymentService } from '../../services/advance-payment.service';
import { NotificationService } from '../../services/notification';
@Component({
  selector: 'app-advance-payment',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './advance-payment.html',
  styleUrls: ['./advance-payment.css']
})

export class AdvancePaymentComponent implements OnInit {
  payments: (AdvancePayment & { laborName?: string })[] = [];
  filteredPayments: (AdvancePayment & { laborName?: string })[] = [];
  labors: any[] = [];
  model: AdvancePayment = this.emptyModel();
  isEdit = false;
  searchText = '';
  page = 1;
  pageSize = 10;

  constructor(private notify: NotificationService, private service: AdvancePaymentService) {}

  ngOnInit() {
    this.loadFormData();
  }

  loadFormData() {
    this.service.getFormData().subscribe({
      next: (res) => {
        this.labors = res || [];
        // Only load payments AFTER we have the labor list to map names
        this.loadPayments(); 
      },
    error: (err) => {
      console.error('API Error (FormData):', err);
      
      // 2. Use the Global Error Notification
      this.notify.showError('Failed to load Advance Payment list. Please ensure the API is running and try again.');
    }
  });
  }

loadPayments() {
  this.service.getAll().subscribe({
    next: (res: any) => {
      
      console.log('Raw API Response:', res); // Check this in console!
      
      // 1. Extract the array regardless of format
      let data = [];
      if (Array.isArray(res)) {
        data = res;
      } else if (res && res.$values) {
        data = res.$values;
      } else {
        data = [];
      }

      // 2. Map the data with safety checks
      this.payments = data.map((p: any) => {
        const laborId = p.laborId ?? p.LaborId;
        const labor = this.labors.find(l => (l.id ?? l.Id) === laborId);
        const rawDate = p.dateGiven ?? p.DateGiven;

        return {
          ...p,
          id: p.id ?? p.Id,
          dateGiven: rawDate ? rawDate.split('T')[0] : '',
          laborName: labor ? (labor.name ?? labor.Name) : 'Unknown',
          laborType: p.laborType ?? p.LaborType ?? 'Permanent'
        };
      });

      this.applyFilter();
    },
    error: (err) => {      
      // 2. Use the Global Error Notification
      this.notify.showError('Failed to load history. Is the API running and the Database updated?');
         }
  });
}

  save() {
    if (!this.model.laborId || !this.model.amount) {
      this.notify.showError('Please fill in all required fields.');
      return;
    }

    const payload: any = {
      ...this.model,
      id: Number(this.model.id),
      laborId: Number(this.model.laborId),
      amount: Number(this.model.amount),
      // Set to 'AutoFetch' - your C# controller will overwrite this with labor.Type
      laborType: this.model.laborType || 'AutoFetch'
    };

    const api$ = this.isEdit
      ? this.service.update(payload.id, payload) 
      : this.service.create(payload);

    api$.subscribe({
      next: () => {
        this.notify.showSuccess(this.isEdit ? 'Expense Updated!' : 'Expense Added!');       
        this.reset();
        this.loadPayments();
      },
      error: (err) => {
          this.notify.close();
      this.notify.showError('Operation failed. Please check your connection.');
      console.error(err);
        }
      
    });
  }

  /* ---------- HELPERS ---------- */

  applyFilter() {
    const search = this.searchText.toLowerCase();
    this.filteredPayments = this.payments.filter(p =>
      p.laborName?.toLowerCase().includes(search) ||
      p.remarks?.toLowerCase().includes(search)
    );
    this.page = 1; 
  }

  paginated() {
    const start = (this.page - 1) * this.pageSize;
    return this.filteredPayments.slice(start, start + this.pageSize);
  }

  totalPages(): number {
    return Math.max(1, Math.ceil(this.filteredPayments.length / this.pageSize));
  }

  prevPage() { if (this.page > 1) this.page--; }
  nextPage() { if (this.page < this.totalPages()) this.page++; }

  get totalAmount() {
    return this.filteredPayments.reduce((a, b) => a + Number(b.amount || 0), 0);
  }

  edit(row: any) {
    this.model = { ...row };
    if (this.model.dateGiven) {
      this.model.dateGiven = this.model.dateGiven.split('T')[0];
    }
    this.isEdit = true;
    window.scrollTo({ top: 0, behavior: 'smooth' });
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
        this.loadPayments();
      },
      error: () => {
        this.notify.close();
        this.notify.showError('Could not delete the item.');
      }
    });
  }
}

  emptyModel(): AdvancePayment {
    return {
      id: 0, laborId: 0, amount: 0,
      dateGiven: new Date().toISOString().split('T')[0],
      remarks: '', laborType: 'Permanent' 
    } as any;
  }

  reset() {
    this.model = this.emptyModel();
    this.isEdit = false;
  }

  success(msg: string) {
    Swal.fire({ icon: 'success', text: msg, timer: 1200, showConfirmButton: false });
  }

  error(msg: string) {
    Swal.fire({ icon: 'error', text: msg });
  }
}