class Result {
    public readonly isSuccess: boolean;
    public readonly error: Error;

    constructor(isSuccess: boolean, error: Error) {
        this.isSuccess = isSuccess;
        this.error = error;
    }

    public get isFailure(): boolean {
        return !this.isSuccess;
    } 
}

export class ResultOf<T> extends Result {
    value: T;
  
    constructor(isSuccess: boolean, error: Error) {
      super(isSuccess,error)
      this.value = {} as T;
    }
  }

export class indexResult<T>{
    values:T[]=[];
    totalRecord=0;
  
    get pages(): number[] {
  
      return Array.from({ length: this.totalRecord }, (_, i) => i + 1);
    }
  
  }