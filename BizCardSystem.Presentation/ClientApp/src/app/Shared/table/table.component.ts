import { Component, EventEmitter, Input, Output, TemplateRef, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';




@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.css']
})
export class TableComponent {
  @Input() data: any[] = [];  // Data passed to the component
  @Input() columns: { field: string, header: string }[] = [];  // Columns definition
  @Input() pageSize: number = 5;  // Default page size
  @Input() pageSizeOptions: number[] = [5, 10, 20];  // Page size options for paginator

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  dataSource: MatTableDataSource<any> = new MatTableDataSource<any>();
  displayedColumns: string[] = [];

  ngAfterViewInit() {
    this.initializeTable();
  }

  ngOnChanges() {
    this.initializeTable();
  }

  initializeTable() {
    this.dataSource = new MatTableDataSource(this.data);
    this.displayedColumns = this.columns.map(column => column.field);
    if (this.paginator) {
      this.dataSource.paginator = this.paginator;
    }
  }

  // Handle the page change event
  onPageChange(event: PageEvent) {
    console.log('Page changed to:', event.pageIndex + 1);
    console.log('Page size:', event.pageSize);
    // Add any additional logic needed on page change
  }
}
 
 