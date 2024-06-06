export default class PlanModel
{
    constructor(
        public id:number,
        public name: string,
        public description: string,
        public oneTimeTotal: number,
        public recurrentTotal: number,
        public characteristics: Map<string, string>,
        public characteristicListValues: Map<string, string>)
    {
    }
}