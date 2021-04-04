import { GeneratedNumber } from "./generated-number.model";
import { MultipliedNumber } from "./multiplied-number.model";

export interface BatchDetail {
  id: number,
  batchId: number,
  batchTotal: number,
  numberOfBatches: number,
  numberPerBatches: number,
  generatedNumbers: GeneratedNumber[]
  multipliedNumbers: MultipliedNumber[]
}
