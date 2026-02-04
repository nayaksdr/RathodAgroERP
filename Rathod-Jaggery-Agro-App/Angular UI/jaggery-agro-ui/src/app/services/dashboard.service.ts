import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DashboardDailyVm } from '../core/models/dashboard-daily.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  private api = `${environment.apiUrl}/api/dashboard`;    

  constructor(private http: HttpClient) {}

  getDashboard(days: number = 14): Observable<DashboardDailyVm> {
    return this.http.get<DashboardDailyVm>(`${this.api}?days=${days}`);
  }
}
