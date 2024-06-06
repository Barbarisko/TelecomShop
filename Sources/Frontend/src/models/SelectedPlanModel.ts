export default class SelectedPlanModel
{
    constructor(
        public id: number,
        public name: string,
        public description: string,
        public priceOneTimeTotal: number,
        public priceRecurrentTotal: number,
        public characteristics: Map<string, string>,
        public characteristicListValues: Map<string, string>,
        public etf: number
    )
    
    {
    }
}