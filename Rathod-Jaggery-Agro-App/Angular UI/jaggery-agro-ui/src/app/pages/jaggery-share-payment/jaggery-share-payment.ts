import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { NotificationService } from '../../services/notification';

@Component({
  selector: 'app-jaggery-share-payment',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule,FormsModule],
  templateUrl: './jaggery-share-payment.html'
})
export class JaggerySharePaymentComponent {

  form: FormGroup;
  selectedFile?: File;
  // Example dropdown data
  members: any[] = [];
  sales: any[] = [];
  model = {
    fromMemberId: null as number | null,
    toMemberId: null as number | null,
    amount: null as number | null,
    paymentMode: 'Cash',
    proofFile: null as File | null
  };

  constructor(
    private fb: FormBuilder,
    private notify: NotificationService,
    private http: HttpClient     
  ) {
    this.form = this.fb.group({
      saleId: ['', Validators.required],
      fromMemberId: ['', Validators.required],
      toMemberId: ['', Validators.required],
      amount: ['', [Validators.required, Validators.min(1)]],
      paymentMode: ['', Validators.required]  
        
    });
  }

  onFile(event: Event) {
  const input = event.target as HTMLInputElement;
  if (input.files && input.files.length > 0) {
    this.selectedFile = input.files[0];
    this.model.proofFile = this.selectedFile; // optional sync
    this.notify.showSuccess('File selected: ' + this.selectedFile.name);
  }
}


  submit() {
  const formData = new FormData();

  if (this.model.fromMemberId !== null)
    formData.append('fromMemberId', String(this.model.fromMemberId));

  if (this.model.toMemberId !== null)
    formData.append('toMemberId', String(this.model.toMemberId));

  if (this.model.amount !== null)
    formData.append('amount', String(this.model.amount));

  formData.append('paymentMode', this.model.paymentMode);

  if (this.selectedFile) {
    formData.append('proofImage', this.selectedFile);
  }

  this.http.post('/api/jaggery-sale-share/record-payment', formData)
    .subscribe({
      next: () => {
        this.notify.showSuccess('Payment recorded successfully');
        this.resetForm();
      },
      error: (err) => {
        this.notify.showError('Failed to record payment');
      }
    });
}
resetForm() {
  this.form.reset();
  this.selectedFile = undefined;
  this.model = {
    fromMemberId: null,
    toMemberId: null,
    amount: null,
    paymentMode: 'Cash',
    proofFile: null
  };
}
}


