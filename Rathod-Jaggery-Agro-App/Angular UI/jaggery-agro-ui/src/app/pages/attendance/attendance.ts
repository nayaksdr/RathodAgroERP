import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgModel } from '@angular/forms';
import Swal from 'sweetalert2';
import { AttendanceService } from '../../services/attendance.service';
import { Attendance } from '../../core/models/Attendance';
import { NotificationService } from '../../services/notification';
@Component({
  selector: 'app-attendance',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './attendance.html',
  styleUrls: ['./attendance.css']
})
export class AttendanceComponent implements OnInit {

  list: Attendance[] = [];
  labors: any[] = [];
  
  model: Attendance = this.emptyModel();
  isEdit = false;

  // Search & Pagination
  searchText = '';
  page = 1;
  pageSize = 10;

  private service = inject(AttendanceService,);
  private notify = inject(NotificationService);

  ngOnInit() {
    this.loadLabors();
    this.loadList();
  }

  /** Load labors for dropdown */
  loadLabors() {
    this.service.getFormData().subscribe(res => this.labors = res);
  }

  /** Load all attendance */
  loadList() {
  this.service.getAll().subscribe({
    next: res => {
      // Map labor names for template display
      this.list = (res || []).map(a => ({
        ...a,
        laborName: this.labors.find(l => l.id === a.laborId)?.name || 'Unknown'
      }));
    },
    error: () => this.notify.showError('Failed to load attendance records. Please try again.')
  });
}


  /** Save / Update attendance */
  save() {
    if (!this.model.laborId || !this.model.attendanceDate) {
      this.notify.showError('Please fill required fields');
      return;
    }

    const api$ = this.isEdit
      ? this.service.update(this.model.id!, this.model)
      : this.service.create(this.model);

    api$.subscribe({
      next: () => {
        this.notify.showSuccess(this.isEdit ? 'Attendance updated!' : 'Attendance recorded!');
       
        this.reset();
        this.loadList();
      },
      error: () => this.notify.showError('Operation failed')
    });
  }

  /** Edit record */
  edit(row: Attendance) {
    this.model = { ...row };
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
        this.loadList(); // Fixed: call loadList() instead of loadPayments()
      },
      error: () => {
        this.notify.close();
        this.notify.showError('Could not delete the item.');
      }
    });
  }
}

  /** Reset form */
  reset() {
    this.model = this.emptyModel();
    this.isEdit = false;
  }

  /** Pagination filtered data */
  paginatedData(): Attendance[] {
    const filtered = this.list.filter(a =>
      this.labors.find(l => l.id === a.laborId)?.name.toLowerCase().includes(this.searchText.toLowerCase()) ||
      (a.isPresent ? 'present' : 'absent').includes(this.searchText.toLowerCase())
    );
    const start = (this.page - 1) * this.pageSize;
    return filtered.slice(start, start + this.pageSize);
  }

  totalPages(): number {
    const filteredLength = this.list.filter(a =>
      this.labors.find(l => l.id === a.laborId)?.name.toLowerCase().includes(this.searchText.toLowerCase()) ||
      (a.isPresent ? 'present' : 'absent').includes(this.searchText.toLowerCase())
    ).length;
    return Math.ceil(filteredLength / this.pageSize);
  }

  private emptyModel(): Attendance {
  return { 
    id: 0,
    laborId: 0,
    date: '',           // <-- required property
    attendanceDate: '', // optional, if you still want it
    isPresent: true 
  };
}

}
