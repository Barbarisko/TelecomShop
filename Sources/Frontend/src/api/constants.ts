export default {
    BASE_URL: "/api",
}

export function mapReplacer(key: any, value: any) {
    if(value instanceof Map) {
        let resObject: any = new Object;
        for(let [k, v] of value.entries())
            {
                resObject[k] = v;
            }

      return resObject
    } else {
      return value;
    }
  }