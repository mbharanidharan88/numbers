import { Component } from '@angular/core';
import { BatchService } from '../../services/batch.service';
import { BatchDetail } from '../../models/batch-detail.model';
import { interval, Subject, Subscription, BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-last-batch-component',
  templateUrl: './last-batch.component.html'
})
export class LastBatchComponent {
  public lastBatchDetails = Array<BatchDetail>();
  private batchSubscription: Subscription;

  constructor(private service: BatchService) {

  }

  ngOnInit() {
    this.loadLastBatch();

 }

  private loadLastBatch() {
    const result = this.service.lastBatch();
      

    this.batchSubscription = result
      .subscribe((res: BatchDetail[]) => {
        console.log('Batch Details...');
        console.log(res);
        //console.log(res['batchDetail']['generatedNumbers']);

        //this.numbers.push(res['batchDetail']['generatedNumbers']);

        this.lastBatchDetails = [...res];
      });
  }
}
