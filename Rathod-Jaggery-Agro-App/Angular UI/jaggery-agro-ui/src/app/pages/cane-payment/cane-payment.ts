import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import Swal from 'sweetalert2';
import { Chart } from 'chart.js/auto';
import { CanePaymentService } from '../../services/cane-payment-service.service';
import { CanePaymentSummary } from '../../core/models/CanePaymentSummary';
import { FormsModule } from '@angular/forms';
import { FarmerService } from '../../services/farmer.service';
import {NotificationService} from'../../services/notification';
@Component({
  selector: 'app-cane-payment-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './cane-payment.html',
  styleUrls: ['./cane-payment.css']
})
export class CanePaymentComponent implements OnInit {

list: CanePaymentSummary[] = [];
  filteredList: CanePaymentSummary[] = [];
  farmers: any[] = []; // NEW: For the dropdown list
today: Date = new Date();
  paidCount = 0;
  pendingCount = 0;
  globalSearch: string = '';
  // Totals for the Dashboard
  totalBalance = 0;
  totalPaidAmount = 0;

  filter = {
    farmerId: null, // Changed from farmerName to farmerId
    from: '',
    to: ''
  };

  chart: any;

  constructor(private service: CanePaymentService,
    private farmerService: FarmerService, // Inject it here
    private notify: NotificationService
  ) {}

  ngOnInit(): void {
    this.load();
    this.loadFarmers();
  }

  loadFarmers() {
    // Re-using the logic from your CanePurchase service to get the master farmer list
    this.farmerService.getAll().subscribe(res => this.farmers = res || []);
  }

  load() {
    this.service.getSummary().subscribe({
      next: (res) => {
      this.list = res || [];
      this.applyFilter();
      this.prepareChart();
    },
     error: (err) => {
      console.error('Error fetching form data:', err);
      this.notify.showError('Failed to load cane payment summary');
     }
    });
  }


 applyFilter() {
  // 1. Prepare Filter Values
  const gSearch = this.globalSearch?.toLowerCase().trim();
  const fromDate = this.filter.from ? new Date(this.filter.from).getTime() : null;
  const toDate = this.filter.to ? new Date(this.filter.to).getTime() : null;

  // 2. Perform Filtering
  this.filteredList = this.list.filter(x => {
    const itemTime = x.paymentDate ? new Date(x.paymentDate).getTime() : null;
    
    // Farmer Dropdown Match
    const matchFarmer = !this.filter.farmerId || x.farmerId == this.filter.farmerId;
    
    // Date Range Match
   const matchFrom = !fromDate || (itemTime && itemTime >= fromDate);
    const matchTo = !toDate || (itemTime && itemTime <= toDate);

    // Global Search Match (Checks Name, ID, Amount, and Date string)
    const matchGlobal = !gSearch || 
      x.farmerName?.toLowerCase().includes(gSearch) || 
      x.id?.toString().includes(gSearch) ||
      x.netAmount?.toString().includes(gSearch);

    return matchFarmer && matchFrom && matchTo && matchGlobal;
  });

  // 3. Update Counts for Pie Chart
  this.paidCount = this.filteredList.filter(x => x.isPaid).length;
  this.pendingCount = this.filteredList.filter(x => !x.isPaid).length;

  // 4. Update Financial Totals (using properties from your latest logic)
  // Logic: Balance = Net - Paid
  this.totalBalance = this.filteredList.reduce((acc, curr) => 
    acc + ((curr.netAmount || 0) - (curr.paidAmount || 0)), 0);
    
  this.totalPaidAmount = this.filteredList.reduce((acc, curr) => 
    acc + (curr.paidAmount || 0), 0);

  // 5. Refresh Visuals
  this.updateChart();
}
  resetFilter() {
    this.filter = { farmerId: null, from: '', to: '' };
    this.applyFilter();
  }

  prepareChart() {
    this.chart = new Chart('paymentChart', {
      type: 'pie',
      data: {
        labels: ['Paid', 'Pending'],
        datasets: [{
          data: [this.paidCount, this.pendingCount],
          backgroundColor: ['#198754', '#dc3545']
        }]
      }
    });
  }

  updateChart() {
    if (!this.chart) return;
    this.chart.data.datasets[0].data = [this.paidCount, this.pendingCount];
    this.chart.update();
  }

  downloadSlip(id: number) {
    this.service.downloadSlip(id).subscribe(blob => {
      const url = URL.createObjectURL(blob);
      window.open(url);
    });
  }

  statusBadge(isPaid: boolean) {
    return isPaid ? 'badge-paid' : 'badge-pending';
  }
}
