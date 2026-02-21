import { Component, computed, input, model, output, OnChanges, SimpleChanges } from '@angular/core';

@Component({
  selector: 'app-paginator',
  standalone: true,
  imports: [],
  templateUrl: './paginator.html',
  styleUrl: './paginator.css',
})
export class Paginator implements OnChanges {
  pageNumber = model(1);
  pageSize = model(10);
  totalCount = input(0);
  totalPages = input(0);
  pageSizeOptions = input([5, 10, 20, 50]);

  pageChange = output<{pageNumber: number, pageSize: number}>();

  // Ye zaroori hai: Jab parent (member-list) se pageSize aaye, toh signal update ho
  ngOnChanges(changes: SimpleChanges) {
    if (changes['pageSize'] && changes['pageSize'].currentValue) {
      this.pageSize.set(changes['pageSize'].currentValue);
    }
  }

  lastItemIndex = computed(() => {
    return Math.min(this.pageNumber() * this.pageSize(), this.totalCount())
  })

  onPageChange(newPage?: number, pageSizeTarget?: EventTarget | null){
    if (newPage) {
      this.pageNumber.set(newPage);
    }
    
    if (pageSizeTarget) {
      const size = Number((pageSizeTarget as HTMLSelectElement).value);
      this.pageSize.set(size);
      this.pageNumber.set(1); 
    } 

    this.pageChange.emit({
      pageNumber: this.pageNumber(),
      pageSize: this.pageSize()
    });
  }
}