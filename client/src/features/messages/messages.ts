import { Component, inject, OnInit, signal } from '@angular/core';
import { MessageService } from '../../core/services/message-service';
import { PaginatedResult } from '../../types/pagination';
import { Message } from '../../types/message';
import { last } from 'rxjs';
import { Paginator } from "../../shared/paginator/paginator";
import { RouterLink } from "@angular/router";
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-messages',
  imports: [Paginator, RouterLink, DatePipe],
  templateUrl: './messages.html',
  styleUrl: './messages.css',
})
export class Messages implements OnInit {
  private messageService = inject(MessageService);
  protected container = 'Inbox';
  protected fetchedContainer = 'Inbox';
  protected pageNumber = 1;
  protected pageSize = 10;
  protected paginatedMessages = signal<PaginatedResult<Message> | null>(null);

  tabs = [
    { label: 'Inbox', value: 'Inbox' },
    { label: 'Outbox', value: 'Outbox' },
  ]

  ngOnInit(): void {
    this.loadMessages();
  }

  loadMessages() {
    this.messageService.getMessages(this.container, this.pageNumber, this.pageSize).subscribe({
      next: response => {
        this.paginatedMessages.set(response);
        this.fetchedContainer = this.container;
      }
    })
  }

 deleteMessage(event: Event, id: string) {
    event.stopPropagation();
    this.messageService.deleteMessage(id).subscribe({
        next: () => {
            this.paginatedMessages.update(prev => {
                if (!prev || !prev.items) return prev;
                return {
                    ...prev,
                    items: prev.items.filter(x => x.id !== id),
                    metadata: prev.metadata 
                };
            });
        }
    });
}

  get isInbox() {
    return this.fetchedContainer === 'Inbox';
  }

  setContainer(container: string) {
    this.container = container;
    this.pageNumber = 1;
    this.loadMessages();
  }

  onPageChange(event: { pageNumber: number, pageSize: number }) {
    this.pageSize = event.pageSize;
    this.pageNumber = event.pageNumber;
    this.loadMessages();
  }
}
