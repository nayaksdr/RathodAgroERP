import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Labor } from '../core/models/labor.model';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class LaborService {
  
    private api = `${environment.apiUrl}/api/labors`; 
  
  constructor(private http: HttpClient) {}

  getAll() {
    return this.http.get<Labor[]>(this.api);
  }

  getById(id: number) {
    return this.http.get<Labor>(`${this.api}/${id}`);
  }

  create(data: Labor) {
    return this.http.post(this.api, data);
  }

  update(id: number, data: Labor) {
    return this.http.put(`${this.api}/${id}`, data);
  }

  delete(id: number) {
    return this.http.delete(`${this.api}/${id}`);
  }
  // ✅ Add this method
  getDropdown(): Observable<any[]> {
    return this.http.get<any[]>(`${this.api}/dropdown`);
  }
}
