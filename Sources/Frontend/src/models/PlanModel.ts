export default class PlanModel
{
    constructor(public name: string,
        public description: string,
        public oneTimeTotal: number,
        public recurrentTotal: number,
        public characteristics: Map<string, string>)
    {
    }
}