import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Attendance } from '../core/models/Attendance';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class AttendanceService {
  private api = `${environment.apiUrl}/api/attendance`; 
 

  constructor(private http: HttpClient) {}

  getAll() {
    return this.http.get<Attendance[]>(this.api);
  }

  getFormData() {
    return this.http.get<any[]>(`${this.api}/form-data`);
  }

  get(id: number) {
    return this.http.get<Attendance>(`${this.api}/${id}`);
  }

  create(data: Attendance) {
    return this.http.post(this.api, data);
  }

  update(id: number, data: Attendance) {
    return this.http.put(`${this.api}/${id}`, data);
  }

  delete(id: number) {
    return this.http.delete(`${this.api}/${id}`);
  }
}
