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
            BRANCHES
          </div>
          <div class="column q-my-lg" style="width: 90%" v-if="matchFound">
            <BrancheComponent
              class="bg-white text-primary"
              v-for="(match, index) in matches.filter((m) => m.stage == 1)"
              :key="index"
              :id="match.id"
              :firstPlayerName="match.firstPlayerName"
              :firstPlayerPoint="match.firstPlayerPoint"
              :secondPlayerName="match.secondPlayerName"
              :secondPlayerPoint="match.secondPlayerPoint"
              :isFinish="match.isFinish"
            />
          </div>
          <div
            class="row bg-primary text-white text-h6 justify-center q-my-md"
            style="height: 40px; width: 30%; border-radius: 10px 10px 10px 10px"
            v-if="matchFound"
          >
            LAST 16
          </div>
          <div class="column q-my-lg" style="width: 90%" v-if="matchFound">
            <BrancheComponent
              class="bg-white text-primary"
              v-for="(match, index) in matches.filter((m) => m.stage == 2)"
              :key="index"
              :id="match.id"
              :firstPlayerName="match.firstPlayerName"
              :firstPlayerPoint="match.firstPlayerPoint"
              :secondPlayerName="match.secondPlayerName"
              :secondPlayerPoint="match.secondPlayerPoint"
              :isFinish="match.isFinish"
            />
          </div>
          <div
            class="row bg-primary text-white text-h6 justify-center q-my-md"
            style="height: 40px; width: 30%; border-radius: 10px 10px 10px 10px"
            v-if="matchFound"
          >
            QUATER-FINAL
          </div>
          <div class="column q-my-lg" style="width: 90%" v-if="matchFound">
            <BrancheComponent
              class="bg-white text-primary"
              v-for="(match, index) in matches.filter((m) => m.stage == 3)"
              :id="match.id"
              :key="index"
              :firstPlayerName="match.firstPlayerName"
              :firstPlayerPoint="match.firstPlayerPoint"
              :secondPlayerName="match.secondPlayerName"
              :secondPlayerPoint="match.secondPlayerPoint"
              :isFinish="match.isFinish"
            />
          </div>
          <div
            class="row bg-primary text-white text-h6 justify-center q-my-md"
            style="height: 40px; width: 30%; border-radius: 10px 10px 10px 10px"
            v-if="matchFound"
          >
            SEMI-FINAL
          </div>
          <div class="column q-my-lg" style="width: 90%" v-if="matchFound">
            <BrancheComponent
              class="bg-white text-primary"
              v-for="(match, index) in matches.filter((m) => m.stage == 4)"
              :key="index"
              :id="match.id"
              :firstPlayerName="match.firstPlayerName"
              :firstPlayerPoint="match.firstPlayerPoint"
              :secondPlayerName="match.secondPlayerName"
              :secondPlayerPoint="match.secondPlayerPoint"
              :isFinish="match.isFinish"
            />
          </div>
          <div
            class="row bg-primary text-white text-h6 justify-center q-my-md"
            style="height: 40px; width: 30%; border-radius: 10px 10px 10px 10px"
            v-if="matchFound"
          >
            FINAL
          </div>
          <div class="column q-my-lg" style="width: 90%" v-if="matchFound">
            <BrancheComponent
              class="bg-white text-primary"
              v-for="(match, index) in matches.filter((m) => m.stage == 5)"
              :key="index"
              :id="match.id"
              :firstPlayerName="match.firstPlayerName"
              :firstPlayerPoint="match.firstPlayerPoint"
              :secondPlayerName="match.secondPlayerName"
              :secondPlayerPoint="match.secondPlayerPoint"
              :isFinish="match.isFinish"
            />
          </div>
          <div
            class="row q-my-lg justify-center text-white text-h4 text-italic"
            style="width: 95%"
            v-if="!matchFound"
          >
            No matches are currently being played.
          </div>
        </div>
      </div>
    </div>
  </q-page>
</template>

<script setup lang="ts">
import BrancheComponent from 'components/BrancheComponent.vue'
import { ref, onMounted } from 'vue'
import type { Match } from '../../components/models'
import { useEventStore } from 'src/stores/event'
import api from 'src/services/api'

const eventStore = useEventStore()

const eventId = eventStore.getEventId

const loading = ref(true)

const matchFound = ref(true)

const matches = ref<Match[]>([])

const fetchMatchs = async () => {
  try {
    const response = await api.get(`/matches/by-event/${eventId}`)
    const data = response.data
    console.log(data)
    matches.value = data
  } catch (error) {
    console.log('Error fetching player: ', error)
    matchFound.value = false
  } finally {
    loading.value = false
  }
}
onMounted(async () => {
  await fetchMatchs()
})
</script>
