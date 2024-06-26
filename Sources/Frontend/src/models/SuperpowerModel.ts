export default class SuperpowerModel {
    constructor(
        public id: number,
        public name: string,
        public description: string,
        public type: string,
        public isActive: boolean,
        public priceOneTimeTotal: number,
        public priceRecurrentTotal: number,
        public characteristics: Map<string, string>,
        public characteristicListValues: Map<string, string>
    ) { }
}