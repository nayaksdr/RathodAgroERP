import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class DepositService {
    private api = `${environment.apiUrl}/api/deposits`;
 
  constructor(private http: HttpClient) {}

  getAll() {
    return this.http.get<any[]>(this.api);
  }

  get(id: number) {
    return this.http.get<any>(`${this.api}/${id}`);
  }

  create(data: any) {
    return this.http.post(this.api, data);
  }
update(id: number, payload: any) {
  return this.http.put(`${this.api}/${id}`, payload);
}

  delete(id: number) {
    return this.http.delete(`${this.api}/${id}`);
  }
}
