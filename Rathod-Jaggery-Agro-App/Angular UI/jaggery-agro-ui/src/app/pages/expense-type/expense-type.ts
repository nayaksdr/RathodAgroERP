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
import { ExpenseTypeService } from '../../services/expense-type.service';
import { NotificationService } from '../../services/notification';

@Component({
  standalone: true,
  selector: 'app-expense-type',
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatIconModule
  ],
  templateUrl: './expense-type.html',
  styleUrls: ['./expense-type.css']
})
export class ExpenseTypeComponent implements OnInit {

  expenseTypes: any[] = [];
  filtered: any[] = []; 

  searchText = '';
  form!: FormGroup;
  isEdit = false;
  selectedId: number | null = null;
  loading = false;

  constructor(
    private fb: FormBuilder,
    private service: ExpenseTypeService,
    private notify: NotificationService
  ) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(2)]],
      description: ['']
    });
    this.load();
  }

  load() {
    this.notify.showLoading('Fetching factory categories...');
    this.service.getAll().subscribe({
      next: (res) => {
        this.expenseTypes = res;
        this.applyFilter(); 
        this.notify.close();
      },
      error: () => this.notify.showError('Failed to load data.')
    });
  }

  applyFilter() {
    const txt = (this.searchText || '').toLowerCase().trim();
    if (!txt) {
      this.filtered = [...this.expenseTypes];
    } else {
      this.filtered = this.expenseTypes.filter(x =>
        x.name.toLowerCase().includes(txt)
      );
    }
  }

  submit() {
    if (this.form.invalid) return;

    this.loading = true;
    this.notify.showLoading(this.isEdit ? 'Updating...' : 'Saving...');
    
    const payload = this.form.value;
    const apiCall = this.isEdit
      ? this.service.update(this.selectedId!, payload)
      : this.service.create(payload);

    apiCall.subscribe({
      next: () => {
        this.loading = false;
        this.notify.close();
        this.notify.showSuccess(this.isEdit ? 'Updated!' : 'Created!');
        this.newForm();
        this.load();
      },
      error: () => {
        this.loading = false;
        this.notify.close();
        this.notify.showError('Operation failed.');
      }
    });
  }

  edit(item: any) {
    this.isEdit = true;
    this.selectedId = item.id;
    this.form.patchValue(item);
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  newForm() {
    this.form.reset();
    this.isEdit = false;
    this.selectedId = null;
    this.loading = false;
  }

  async delete(id: number) {
    const result = await this.notify.confirm('Are you sure?', 'Delete this factory category?');
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
          this.notify.showError('Could not delete.');
        }
      });
    }
  }

cancelEdit() {
    this.newForm();
    this.notify.showSuccess('Action cancelled'); // Optional: feedback for the user
  }
  getIcon(name: string): string {
    const n = (name || '').toLowerCase();
    if (n.includes('sugarcane')) return 'agriculture';
    if (n.includes('fuel') || n.includes('firewood')) return 'local_fire_department';
    if (n.includes('labor')) return 'engineering';
    return 'inventory_2';
  }
}