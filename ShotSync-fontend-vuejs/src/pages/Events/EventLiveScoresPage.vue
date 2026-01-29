<template>
  <q-page class="row justify-center">
    <div class="col justify-center items-center">
      <div class="row justify-center items-center" v-if="loading" style="height: 60vh">
        <q-spinner-cube color="orange" size="5.5em" />
      </div>
    </div>
    <div class="col-12 q-py-md column" v-if="!loading">
      <div
        class="event-infor-component w-100 bg-no-2 q-my-sm row"
        style="height: auto; border-radius: 20px; width: 100%"
      >
        <div class="col column justify-center items-center">
          <div
            class="row bg-primary text-white text-h6 justify-center"
            style="height: 40px; width: 30%; border-radius: 0 0 10px 10px"
          >
            LIVE SCORES
          </div>
          <div class="row q-my-lg justify-center" style="width: 95%" v-if="matchFound">
            <LiveScoreComponent
              v-for="(match, index) in matchs"
              :key="index"
              :table="match.table"
              :firstPlayerName="match.firstPlayerName"
              :secondPlayerName="match.secondPlayerName"
              :firstPoint="match.firstPlayerPoint"
              :secondPoint="match.secondPlayerPoint"
              :isStart="match.isStart"
              :isFinish="match.isFinish"

            />
          </div>
          <div class="row q-my-lg justify-center text-white text-h4 text-italic" style="width: 95%" v-if="!matchFound" >
            No matches are currently being played.
          </div>
        </div>
      </div>
    </div>
  </q-page>
</template>

<script setup lang="ts">
import LiveScoreComponent from 'components/LiveScoreComponent.vue'
import { ref, onMounted } from 'vue'
import type { Match } from '../../components/models';
import { useEventStore } from 'src/stores/event';
import api from 'src/services/api';

const eventStore = useEventStore()

const eventId = eventStore.getEventId

const loading = ref(true)

const matchFound = ref(true)

const matchs = ref<Match[]>([]);

const fetchPlayers = async () => {
  try {
    const response = await api.get(`/matches/by-event/${eventId}`)
    const data = response.data
    matchs.value = data
  } catch (error) {
    console.log("Error fetching player: ",error);
    matchFound.value = false
  } finally {
    loading.value = false
  }
}
onMounted(async () => {
  await fetchPlayers()
})

</script>
