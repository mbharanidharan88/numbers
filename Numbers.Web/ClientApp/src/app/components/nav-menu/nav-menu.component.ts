import { Component } from '@angular/core';
import { BatchService } from '../../services/batch.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;

  public showLastBatchMenuItem = false;

  constructor(private batchService: BatchService) {

  }

  ngOnInit() {
    this.initSubscriptions();
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  private initSubscriptions() {
    this.batchService.lastBatchMenuSubject.asObservable()
    .subscribe(result => {
      console.log('resulttt', result);
      this.showLastBatchMenuItem = result;
    });
  }
}
