import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Chart, registerables } from 'chart.js';
import { forkJoin } from 'rxjs';

/* Angular Material */
import { MatIconModule } from '@angular/material/icon';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatButtonModule } from '@angular/material/button';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatMenuModule } from '@angular/material/menu'; // Added for professional UI

import { NotificationService } from '../../services/notification';
import { JaggerySaleService } from '../../services/jaggery-sale.service';

Chart.register(...registerables);

@Component({
  selector: 'app-jaggery-share-dashboard',
  standalone: true,
  imports: [
    CommonModule, FormsModule, ReactiveFormsModule, MatIconModule, 
    MatProgressBarModule, MatButtonModule, MatTooltipModule, 
    MatProgressSpinnerModule, MatMenuModule
  ],
  templateUrl: './jaggery-share-dashboard.html',
  styleUrl: './jaggery-share-dashboard.css'
})
export class JaggeryShareDashboardComponent implements OnInit, OnDestroy {
  // Stats
  stats = { totalSales: 0, totalAdvance: 0, totalRemaining: 0, totalQty: 0 };
  isLoading = false;
  isEditMode = false;
  selectedShareId: number | null = null;
  Math = Math; // Required for Math.abs in template

  // Data Lists
  sales: any[] = [];
  members: any[] = [];
  recordedShares: any[] = [];
  dealerBalances: any[] = [];
  settlementMatrix: any[] = [];
  memberSummary: any[] = []; // Unified partner stats
  memberCategoryBreakdown: any[] = []; // For the detailed table

  // Form Model
  model = {
    jaggerySaleId: null as any,
    paidAmount: 0,
    splitMemberIds: [] as number[],
    individualShare: 0,
    paymentMode: 'Bank',
    attachmentBase64: ''
  };

  private mainChart: Chart | null = null;

  constructor(private notify: NotificationService, private api: JaggerySaleService) {}

  ngOnInit(): void { this.loadAllData(); }

  loadAllData() {
    this.isLoading = true;
    forkJoin({
      members: this.api.getMembers(),
      report: this.api.getSales(),
      shares: this.api.getSaleShares(),
      expenses: this.api.getExpenses(),
      advances: this.api.getDealerAdvances() // Added this crucial source
    }).subscribe({
      next: (res: any) => {
        this.members = res.members || [];
        this.recordedShares = res.shares || [];
        
        if (res.report) {
          this.sales = res.report.sales || [];
          this.stats = {
            totalSales: res.report.summary?.totalAmount || 0,
            totalAdvance: res.report.summary?.totalAdvance || 0,
            totalRemaining: res.report.summary?.totalRemaining || 0,
            totalQty: res.report.summary?.totalQty || 0
          };
          this.mapDealerProgress();
        }
        // Inside loadAllData subscribe next block...
if (res.report && res.report.sales) {
  this.sales = res.report.sales.map((s: any) => {
    // Find the member name from the members list using paidById
    const paidByMember = this.members.find(m => m.id === s.paidById);
    const memberName = paidByMember ? paidByMember.name : 'Unknown';

    return {
      ...s,
      // Create a descriptive text for the dropdown
      displayText: `${s.dealer?.name || 'No Dealer'} | By: ${memberName} | ₹${s.remainingAmount} (${new Date(s.saleDate).toLocaleDateString()})`
    };
  });
}

        // Run the master calculation engine
        this.calculateUnifiedFinancials(res.expenses, res.shares, res.advances);
        
        setTimeout(() => this.renderChart(), 300);
        this.isLoading = false;
      },
      error: () => {
        this.notify.showError('System sync failed');
        this.isLoading = false;
      }
    });
  }
onSaleSelect(event: any) {
  const saleId = event.target.value;
  // This will find the sale including the dealer and amount info
  const selected = this.sales.find(s => s.id == saleId); 
  
  if (selected) {
    // Automatically set the amount to the remaining balance of that specific dealer sale
    this.model.paidAmount = selected.remainingAmount;
    this.calculateShare();
  }
}
 calculateUnifiedFinancials(expenses: any[], shares: any[], advances: any[]) {
  const statsMap = new Map<string, any>();

  // 1. Initialize stats for each member
  this.members.forEach(m => {
    statsMap.set(m.name, { 
      name: m.name, 
      id: m.id,
      spent: 0,        // Expenses + Dealer Advances they paid
      taken: 0,        // Profit/Cash they actually received
      netBalance: 0 
    });
  });

  // 2. Add up what they SPENT (Out of pocket)
  expenses.forEach(e => {
    const m = statsMap.get(e.paidBy?.name);
    if (m) m.spent += e.amount;
  });
  advances.forEach(a => {
    const m = statsMap.get(a.paidByName);
    if (m) m.spent += a.amount;
  });

  // 3. Add up what they TOOK (Income/Shares)
  shares.forEach(s => {
    const m = statsMap.get(s.name);
    if (m) m.taken += s.paidAmount;
  });

  // 4. Calculate Net Position
  // Formula: What I spent - What I took. 
  // (+) means the business owes me. (-) means I owe the business.
  this.memberSummary = Array.from(statsMap.values()).map(m => {
    m.netBalance = m.spent - m.taken;
    // Correct Percentage: How much of my spending have I recovered?
    m.recoveryPercent = m.spent > 0 ? (m.taken / m.spent) * 100 : 0;
    return m;
  });

  this.runSettlementAlgorithm();
}

runSettlementAlgorithm() {
  // Goal: Everyone should end up at the same "Net" (Profit/Loss share)
  const totalNet = this.memberSummary.reduce((sum, m) => sum + m.netBalance, 0);
  const targetPerMember = totalNet / (this.members.length || 1);

  const diffs = this.memberSummary.map(m => ({
    name: m.name,
    amount: m.netBalance - targetPerMember
  }));

  const creditors = diffs.filter(x => x.amount > 0); // They need to RECEIVE money
  const debtors = diffs.filter(x => x.amount < 0);   // They need to PAY money

  this.settlementMatrix = [];

  debtors.forEach(d => {
    let toPay = Math.abs(d.amount);
    creditors.forEach(c => {
      if (toPay > 0 && c.amount > 0) {
        let payment = Math.min(toPay, c.amount);
        this.settlementMatrix.push({ from: d.name, to: c.name, amount: Math.round(payment) });
        toPay -= payment;
        c.amount -= payment;
      }
    });
  });
}

  mapDealerProgress() {
    this.dealerBalances = this.sales.map(s => ({
      name: s.dealer?.name || 'Unknown',
      status: s.remainingAmount, // Negative implies we owe dealer, Positive implies dealer owes us
      percent: s.totalAmount > 0 ? (s.advancePaid / s.totalAmount) * 100 : 0
    }));
  }

  // --- UI ACTIONS ---
  generateSettlementMessage(member: any) {
    const action = member.netBalance < 0 ? 'PAY' : 'RECEIVE';
    const msg = `📊 *Settlement Summary: ${member.name}*\n- Net Position: *${action} ₹${Math.abs(member.netBalance).toLocaleString()}*`;
    navigator.clipboard.writeText(msg);
    this.notify.showSuccess('Copied to WhatsApp');
  }

  printReport() { window.print(); }

  viewReceipt(member: any): void {
    if (member.attachmentUrl) {
      window.open(member.attachmentUrl, '_blank');
    } else if (member.attachmentBase64) {
      const win = window.open();
      win?.document.write(`<img src="${member.attachmentBase64}" style="width:100%">`);
    } else {
      this.notify.showError(`No proof found for ${member.name}`);
    }
  }
 
  toggleMember(id: number) {
    const index = this.model.splitMemberIds.indexOf(id);
    if (index > -1) {
      this.model.splitMemberIds.splice(index, 1);
    } else {
      this.model.splitMemberIds.push(id);
    }
    this.calculateShare();
  }

  calculateShare() {
    const count = this.model.splitMemberIds.length;
    this.model.individualShare = count > 0 ? this.model.paidAmount / count : 0;
  }

  onFileChange(event: any) {
    const file = event.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.onload = () => this.model.attachmentBase64 = reader.result as string;
      reader.readAsDataURL(file);
    }
  }

  onSubmit() {
    if (!this.model.jaggerySaleId || this.model.splitMemberIds.length === 0) {
      this.notify.showError('Please select a sale and partners.');
      return;
    }

    if (this.isEditMode && this.selectedShareId !== null) {
      this.api.updateShare(this.selectedShareId, this.model).subscribe({
        next: () => this.handleSuccess('Updated'),
        error: () => this.notify.showError('Update failed')
      });
    } else {
      this.api.recordShare(this.model).subscribe({
        next: () => this.handleSuccess('Recorded'),
        error: () => this.notify.showError('Save failed')
      });
    }
  }

  private handleSuccess(msg: string) {
    this.notify.showSuccess(msg);
    this.resetForm();
    this.loadAllData();
  }

  resetForm() {
    this.isEditMode = false;
    this.selectedShareId = null;
    this.model = { jaggerySaleId: null, paidAmount: 0, splitMemberIds: [], individualShare: 0, paymentMode: 'Bank', attachmentBase64: '' };
  }

  renderChart() {
    if (this.mainChart) this.mainChart.destroy();
    const ctx = document.getElementById('mainChart') as HTMLCanvasElement;
    if (!ctx) return;
    
    this.mainChart = new Chart(ctx, {
      type: 'bar', // Bar chart is better for comparing contributions
      data: {
        labels: this.memberSummary.map(m => m.name),
        datasets: [
          { 
            label: 'Net Balance', 
            data: this.memberSummary.map(m => m.netBalance),
            backgroundColor: this.memberSummary.map(m => m.netBalance < 0 ? '#f87171' : '#34d399')
          }
        ]
      },
      options: {
        responsive: true,
        plugins: { legend: { display: false } }
      }
    });
  }

  ngOnDestroy() { this.mainChart?.destroy(); }
}