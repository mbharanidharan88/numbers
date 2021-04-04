import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { HttpClient, HttpParams } from '@angular/common/http';
import { interval, Subject, Subscription, BehaviorSubject } from 'rxjs';
import { switchMap, map, takeUntil, tap } from 'rxjs/operators';
import { BatchService } from '../../services/batch.service';
import { BatchRequest } from '../../models/batchrequest.model';
import { BatchResponse } from '../../models/batch-response.model';
import { Batch } from '../../models/batch.model';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {

  public batchForm: FormGroup;
  public numberOfBatches = new  FormControl('1');
  public numbersPerBatch = new FormControl('2');
  private ngUnsubscribe = new Subject();
  public numbers = new Array<number>();
  public batches = new Array<Batch>();
  public CurrentStatus: string;
  public canClear: boolean = false;

  public newBatchSubject: Subject<boolean> = new BehaviorSubject<boolean>(true);
  public startNewBatch = this.newBatchSubject.asObservable();
  public canStartNewBatch: boolean = true;

  private pollingSubscription: Subscription;
  private pollingInterval: number = 2000; //in ms

  constructor(private service: BatchService) {
  }

  ngOnInit() {
    this.initForm();
    this.initSubscriptions();
  }

  start() {

    //console.log(!this.CurrentStatus);
    //console.log(this.CurrentStatus == BatchStatus[BatchStatus.Completed]);
    //console.log(!this.CurrentStatus || this.CurrentStatus == BatchStatus[BatchStatus.Completed]);
    
    // if (!this.CurrentStatus || this.CurrentStatus == BatchStatus[BatchStatus.Completed]) {

    console.log('this.canStartNewBatch' + this.canStartNewBatch);

    if ( this.canStartNewBatch) {

      console.log('Batch Started');

      this.newBatchSubject.next(false);

      this.service.startBatch(this.batchForm.value)
        .subscribe(result => {
          console.log('result');
          console.log(result);
          this.poll(result);
        });
    }
  }

  clear() {
    console.log('Batch Cleared...');
    //this.ngUnsubscribe.next();
    //this.ngUnsubscribe.complete();

    //this.pollingSubscription.unsubscribe();

    this.service.clear()
      .subscribe(result => {

        console.log('clear result', result);

        if (result) {
          this.newBatchSubject.next(true);
          this.batches = [];
          this.initForm();
        }
        
      });
  }

  private poll(sessionId) {

    console.log('Polling Started...')

    const result = interval(this.pollingInterval)
      .pipe(
        //switchMap(() => this.http.get<BatchResponse>(this.baseUrl + 'batch/' + batchId)),

        switchMap(() => this.service.pollBatchData(sessionId)),
        //tap((res: BatchResponse) => console.log(res)),
        //map(res => res),
        takeUntil(this.ngUnsubscribe)
      )

    this.pollingSubscription = result
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe((res: BatchResponse) => {
        console.log('Polling...');
        console.log(res);
        //console.log(res['batchDetail']['generatedNumbers']);

        //this.numbers.push(res['batchDetail']['generatedNumbers']);

        this.updateStatus(res.currentStatus);
        this.batches = [...res.responseData];
      });
  }

  private updateStatus(currentStatus) {

    if (currentStatus == BatchStatus.Completed) {
      this.canClear = true;
      this.stopPolling();
    }

    this.CurrentStatus = BatchStatus[currentStatus];
  }

  private stopPolling() {

    if (this.pollingSubscription) {
      console.log('Polling Stopped...');
      this.pollingSubscription.unsubscribe();
    }
  }

  private processPollData() {

  }

  private initForm() {
    this.batchForm = new FormGroup({
      numberOfBatches: new FormControl(null, [Validators.required]),
      numbersPerBatch: new FormControl(null, [Validators.required])
    });
  }

  private initSubscriptions() {
    this.startNewBatch.subscribe(value => this.canStartNewBatch = value);
  }
}


export enum BatchStatus {
  Started = 0,
  GeneratingNumbers = 1,
  MultiplyingNumbers = 2,
  Completed = 3
}
