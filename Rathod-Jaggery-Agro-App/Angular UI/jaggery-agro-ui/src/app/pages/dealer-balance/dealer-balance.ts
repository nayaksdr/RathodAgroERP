import { Component } from '@angular/core';
import { JaggerySaleService } from '../../services/jaggery-sale.service';
import { CommonModule } from '@angular/common';
import { NotificationService } from '../../services/notification';
@Component({
  standalone: true,
  selector: 'app-dealer-balance',
  imports: [CommonModule],
  templateUrl: './dealer-balance.html'
})
export class DealerBalanceComponent {

  dealer: any;
  sales: any[] = [];

  constructor(private service: JaggerySaleService, private notify: NotificationService) {}

 load(dealerId: number) {
  // ✅ Pass dealerId directly as a number, not an object
  this.service.getSales(dealerId).subscribe({
    next: (res: any[]) => {
      this.sales = res || [];
      this.notify.showLoading(this.sales.length + ' sales records loaded for dealer ID ' + dealerId);
      // If you have a summary calculation, call it here:
      // this.calculateSummary(); 
    },
    error: (err) => 
      this.notify.showError('Failed to load sales records: ' + err.message)
      
  });
}
}
