import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormsModule,
  ReactiveFormsModule,
  FormBuilder,
  Validators,
  FormGroup
} from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import Swal from 'sweetalert2';
import { DepositService } from '../../services/deposit.service';
import { NotificationService } from '../../services/notification';
@Component({
  standalone: true,
  selector: 'app-deposit',
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatIconModule
  ],
  templateUrl: './deposit.html'
})
export class DepositComponent implements OnInit {

  // List
  deposits: any[] = [];
  filtered: any[] = [];

  // Search + Pagination
  searchText = '';
  page = 1;
  pageSize = 5;

  // Form
  form!: FormGroup;
  isEdit = false;
  selectedId: number | null = null;
  loading = false;

  constructor(
    private fb: FormBuilder,
    private notify: NotificationService,
    private service: DepositService
  ) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      depositorName: ['', Validators.required],
      amount: ['', [Validators.required, Validators.min(1)]],
      depositDate: ['', Validators.required],
      remarks: ['']
    });
this.notify.showLoading('Loading deposits...');
    this.load();
  }

  load() {
    this.service.getAll().subscribe(res => {
      this.deposits = res;
      this.applyFilter();
    });
  }

  // 🔍 Search
  applyFilter() {
    const txt = this.searchText.toLowerCase();
    this.filtered = this.deposits.filter(x =>
      x.depositorName.toLowerCase().includes(txt)
    );
    this.page = 1;
  }

  // 📄 Pagination
  get paginatedData() {
    const start = (this.page - 1) * this.pageSize;
    return this.filtered.slice(start, start + this.pageSize);
  }

  totalPages() {
    return Math.ceil(this.filtered.length / this.pageSize) || 1;
  }

  // ➕ New
  newForm() {
    this.form.reset();
    this.isEdit = false;
    this.selectedId = null;
  }

  // ✏ Edit
  edit(item: any) {
    this.isEdit = true;
    this.selectedId = item.id;
    this.form.patchValue(item);
this.notify.showSuccess('Editing deposit for ' + item.depositorName);  

    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  // 💾 Save / Update
  submit() {
    if (this.form.invalid) return;

    this.loading = true;
    const payload = this.form.value;

    const apiCall = this.isEdit
      ? this.service.update(this.selectedId!, payload)
      : this.service.create(payload);

    apiCall.subscribe(() => {
      this.loading = false;
      this.notify.close(); // Close loading notification
this.notify.showSuccess(this.isEdit ? 'Deposit updated!' : 'Deposit created!');
     
      this.newForm();
      this.load();
    });
  }

  // ❌ Cancel
  cancelEdit() {
    this.notify.close(); // Close any existing notifications
    this.newForm();
  }
// --- 3. DELETE (With Confirmation) ---
async delete(id: number) {
  // Wait for user confirmation
  const result = await this.notify.confirm(
    'Are you sure?', 
    'You will not be able to recover this deposit record!'
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
