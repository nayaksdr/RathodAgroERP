import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class DealerService {

  private api = `${environment.apiUrl}/api/dealers`;

  constructor(private http: HttpClient) {}

  getAll(search = '', page = 1) {
    return this.http.get<any>(
      `${this.api}?search=${search}&page=${page}`
    );
  }

  get(id: number) {
    return this.http.get<any>(`${this.api}/${id}`);
  }
getDropdown() {
  return this.http.get<any[]>(`${this.api}/dropdown`);
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
