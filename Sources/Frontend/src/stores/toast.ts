import { ref, computed } from 'vue'
import { defineStore } from 'pinia'

export enum ToastLevel
{
  Info,
  Warning,
  Error
}
export interface ToastData {
  title: string,
  message: string,
  level: ToastLevel
}

export const useToastStore = defineStore('toast', () => {
  const data = ref<Map<number, ToastData>>(new Map<number, ToastData>());
  var lastId = 0;

  function showToast(info: ToastData) {
    let newId = lastId = lastId + 1;
    data.value.set(newId, info);
  }

  return { data, showToast }
})
