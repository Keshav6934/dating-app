import { Component, inject, OnInit, signal, ViewChild } from '@angular/core';
import { MemberService } from '../../../core/services/member-service';
import { Member, MemberParams } from '../../../types/member';
import { MemberCard } from "../member-card/member-card";
import { PaginatedResult } from '../../../types/pagination';
import { Paginator } from "../../../shared/paginator/paginator";
import { FilterModal } from '../filter-modal/filter-modal';

@Component({
  selector: 'app-member-list',
  standalone: true,
  imports: [MemberCard, Paginator, FilterModal],
  templateUrl: './member-list.html',
  styleUrl: './member-list.css',
})
export class MemberList implements OnInit {
  @ViewChild('filterModal') modal!: FilterModal;
  private memberService = inject(MemberService);
  protected paginatedMembers = signal<PaginatedResult<Member> | null>(null);
  protected memberParams = new MemberParams();
  protected updatedParams = new MemberParams();

  constructor() {
    const filters = localStorage.getItem('filters');
    if (filters) {
      const parsed = JSON.parse(filters);
      this.memberParams = parsed;
      this.updatedParams = parsed;
    }
  }

  ngOnInit(): void {
    this.loadMembers();
  }

  loadMembers() {
    this.memberService.getMembers(this.memberParams).subscribe({
      next: result => {
        this.paginatedMembers.set(result);
      }
    });
  }

  onPageChange(event: { pageNumber: number, pageSize: number }) {
    this.memberParams.pageSize = event.pageSize;
    this.memberParams.pageNumber = event.pageNumber;
    this.loadMembers();
  }

  openModal() {
    this.modal.open();
  }

  onClose() {
    console.log('Modal closed');
  }

  onFilterChange(data: MemberParams) {
    this.memberParams = { ...data };
    this.updatedParams = { ...data };
    this.loadMembers();
  }

  resetFilters() {
    const freshParams = new MemberParams();
    freshParams.minAge = 18;
    freshParams.maxAge = 100;
    
    this.memberParams = { ...freshParams };
    this.updatedParams = { ...freshParams };
    
    if (this.modal) {
      this.modal.memberParams.set({ ...freshParams });
    }

    localStorage.removeItem('filters');
    this.loadMembers();
  }

  get displayMessage(): string {
    const filters: string[] = [];

    if (this.updatedParams.gender) {
      filters.push(this.updatedParams.gender === 'male' ? 'Males' : 'Females');
    } else {
      filters.push('Males, Females');
    }

    if (this.updatedParams.minAge !== 18 || this.updatedParams.maxAge !== 100) {
      filters.push(`Ages ${this.updatedParams.minAge} - ${this.updatedParams.maxAge}`);
    }

    filters.push(this.updatedParams.orderBy === 'lastActive' ? 'Recently active' : 'Newest members');

    return `Selected: ${filters.join(' | ')}`;
  }
}