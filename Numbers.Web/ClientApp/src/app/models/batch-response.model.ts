import { Batch } from "./batch.model";

export interface BatchResponse {
  batchesTotal: number,
  currentStatus: any,
  responseData: Batch[]
}
