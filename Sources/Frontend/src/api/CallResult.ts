export default class CallResult<T> {
    constructor(public success: boolean,
        public error: string,
        public data: T | undefined = undefined ) { }

    Get(): T{
        if (!this.success || this.data === undefined) {
            throw new Error("No valid data");
        }
        return this.data as T
    }
}