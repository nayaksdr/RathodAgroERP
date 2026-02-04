import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { PaymentService } from '../../services/payments';
import { NotificationService } from '../../services/notification';
@Component({
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatTableModule,
    MatButtonModule,
    MatInputModule,
    MatCardModule
  ],
  templateUrl: './payments.html',
  styleUrls: ['./payments.css']
})
export class PaymentsComponent {

  list: any[] = [];
  from!: string;
  to!: string;

  constructor(private api: PaymentService,private notify: NotificationService) {}

  load() {
    this.api.getAll(this.from, this.to).subscribe(res => this.list = res);
  }

  generate() {
    this.api.generate({
      fromDate: this.from,
      toDate: this.to,
      paymentMethod: 'Cash'
    }).subscribe(res => {
      this.list = res;
      this.notify.showSuccess('Payments Generated Successfully');
      
    });
  }

  download(id: number) {
    this.api.downloadSlip(id).subscribe(blob => {
      const url = window.URL.createObjectURL(blob);
      window.open(url);
      this.notify.showSuccess('Payment Slip Downloaded');
    });
  }
}
