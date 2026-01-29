import { defineStore, acceptHMRUpdate } from 'pinia';

export const useEventStore = defineStore('eventStore', {
  state: () => ({
    eventId: localStorage.getItem('eventId') ? Number(localStorage.getItem('eventId')) : 0,
  }),
  getters: {
    getEventId: (state) => state.eventId,
  },
  actions: {
    setEventId(id: number){
      this.eventId = id;
      localStorage.setItem('eventId', id.toString())
    }
  }
});

if (import.meta.hot) {
  import.meta.hot.accept(acceptHMRUpdate(useEventStore, import.meta.hot));
}
