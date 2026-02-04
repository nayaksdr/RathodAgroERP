import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Inject } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { LaborService } from '../../services/labor.service';
import { Labor } from '../../core/models/labor.model';
import { LaborTypeService } from '../../services/labor-type.service';
import { NotificationService } from '../../services/notification';

@Component({
  selector: 'app-labor',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatCardModule,
    MatButtonModule,
    MatInputModule,
    MatIconModule,
    MatTableModule,
    MatPaginatorModule,
    MatFormFieldModule,
    MatSelectModule   
  ],
  templateUrl: './labor.html',
  styleUrls: ['./labor.css']  // ✅ fixed
})
export class LaborComponent implements OnInit, AfterViewInit {

  dataSource = new MatTableDataSource<Labor>();

  displayedColumns: string[] = [
    'name',
    'mobile',
    'status',
    'actions'
  ];
pageSize = 5;

  laborTypes: any[] = [];   // 👈 ADD THIS
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  model: Labor = {
    name: '',
    mobile: '',
    laborTypeId: 0,
    isActive: true
  };
 currentPage = 1;

  showForm = false; // ✅ Add this
  constructor( private service: LaborService,
  private laborTypeService: LaborTypeService,private notify: NotificationService ){}  // 👈 ADD THIS) {}

  loadLaborTypes(): void {
  this.laborTypeService.getAll().subscribe({
    next: (res) => {
      this.laborTypes = res;
      this.notify.close(); // Close loading notification
    },
    error: (err) => {
      this.notify.showError('Failed to load labor');
      console.error('Error loading labor types:', err);
    }
  });
}
  // ================= INIT =================
  ngOnInit(): void {
    this.load();
 this.loadLaborTypes();
    // 🔍 Custom Filter (name + mobile only)
    this.dataSource.filterPredicate = (data: Labor, filter: string) => {
      const search = filter.trim().toLowerCase();

      const name = data.name?.toLowerCase() || '';
      const mobile = data.mobile?.toLowerCase() || '';

      return name.includes(search) || mobile.includes(search);
    };
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
  }

  // ================= LOAD =================
  load(): void {
    this.service.getAll().subscribe({
      next: (res) => {
        this.dataSource.data = res;
        
      },
      error: (err) => {
        this.notify.showError('Failed to load labor list. Please try again.');
        console.error('Error loading labors:', err);
      }
    });
  }

  // ================= SAVE =================
  save(): void {

    if (!this.model.name?.trim() || !this.model.mobile?.trim()) {
      this.notify.showError('Please fill all required fields');      
      return;
    }

    if (this.model.id) {
      this.service.update(this.model.id, this.model)
        .subscribe({
          next: () => this.afterSave(),
          error: err => {
            this.notify.showError('Failed to save labor details.');
            console.error(err);
          }
        });
    } else {
      this.service.create(this.model)
        .subscribe({
          next: () => this.afterSave(),
          error: err => {
            this.notify.showError('Failed to create labor details.');
            console.error(err);
          }
        });
    }
  }

 // ================= EDIT =================
edit(labor: Labor): void {
  this.model = { ...labor };
    this.showForm = true; 
  window.scrollTo({ top: 0, behavior: 'smooth' });
}


// ================= VIEW =================
view(labor: Labor): void {
  alert(`
  Name: ${labor.name}
  Mobile: ${labor.mobile}
  Type: ${labor.laborTypeId}
  Status: ${labor.isActive ? 'Active' : 'Inactive'}
  `);
   
}


// ================= DELETE =================
async delete(id: number) {
  // Wait for user confirmation
  const result = await this.notify.confirm(
    'Are you sure?', 
    'You will not be able to recover this expense record!'
  );

  if (result.isConfirmed) {
    this.notify.showLoading('Deleting...');

    this.service.delete(id).subscribe({
      next: () => {
        this.notify.close();
        this.notify.showSuccess('Deleted successfully.');
        this.load();
      },
      error: () => {
        this.notify.close();
        this.notify.showError('Could not delete the item.');
      }
    });
  }
}  

  // ================= CLEAR =================
  clear(): void {
  this.model = {
    id: 0,              // Important if editing
    name: '',
    mobile: '',
    laborTypeId: null,  // Better than 0
    isActive: true
  };
   this.showForm = true; 
}

  afterSave(): void {
    this.clear();
    this.load();
  }

  // ================= SEARCH =================
  applyFilter(event: Event): void {
    const filterValue = (event.target as HTMLInputElement).value;

    this.dataSource.filter = filterValue.trim().toLowerCase();

    // ✅ Reset to first page after filtering
    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }
  

get totalPages(): number {
  return Math.ceil(this.dataSource.data.length / this.pageSize);
}

get totalPagesArray(): number[] {
  return Array(this.totalPages).fill(0).map((x,i)=>i+1);
}

prevPage(): void {
  if(this.currentPage > 1) this.currentPage--;
}

nextPage(): void {
  if(this.currentPage < this.totalPages) this.currentPage++;
}

goToPage(page: number): void {
  this.currentPage = page;
}

}
