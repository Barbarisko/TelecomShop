import { ref, computed } from 'vue'
import { defineStore } from 'pinia'
import * as jwt_decode from 'jwt-decode';

interface Payload {
  name: string,
  surname: string,
  phoneNumber: string
}

interface State {
  token: string
  parsedObj: jwt_decode.JwtPayload | undefined
  user: Payload | undefined
}

export const useLoginStore = defineStore('login', () => {
  const data = ref({
    token: "",
    user: undefined
  } as State);

  const isLoggedIn = computed(() => data.value.token != "")
  const name = computed(() => data.value.user ? data.value.user.name : "")
  const surname = computed(() => data.value.user ? data.value.user.surname : "")
  const mobilePhone = computed(() => data.value.user ? data.value.user.phoneNumber : "")


  function logOut() {
    data.value.token = "";
    data.value.user = undefined;
  }

  function logIn(token: string): boolean {
    try {
      var parsedObj = JSON.parse(JSON.stringify(jwt_decode.jwtDecode(token)));
      data.value.user = {
        name: parsedObj["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"],
        surname: parsedObj["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname"],
        phoneNumber: parsedObj["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/mobilephone"]
      } as Payload;
      data.value.token = token;
      return true;
    } catch (error) {
      return false;
    }
  }

  return { data, isLoggedIn, name, surname, mobilePhone, logOut, logIn }
},
{
  persist: true,
})
