import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { NotificationService } from '../../services/notification';
@Component({
  selector: 'app-farmer-report.component',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './farmer-report.component.html',
  styleUrl: './farmer-report.component.css',
})
@Component({
  standalone: true,
  imports: [CommonModule],
  templateUrl: './farmer-report.component.html'
})
export class FarmerReportComponent {

  report: any;

  constructor(private http: HttpClient, private notify: NotificationService) {}

  load(id: number) {
    this.http.get(`/api/cane-purchases/farmer-report/${id}`)
      .subscribe({
        next: (res) => {
          this.report = res;
        },
        error: (err) => {
          this.notify.showError('Failed to load farmer report');
        }
      });
  }
}
