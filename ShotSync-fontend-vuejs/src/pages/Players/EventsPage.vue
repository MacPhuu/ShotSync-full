<template>
  <q-page class="row items-center justify-evenly">
    <div class="row q-pt-lg">
      <div class="col justify-center">
        <div class="row justify-center items-center" v-if="loading" style="height: 60vh">
          <q-spinner-cube color="orange" size="10.5em" />
        </div>
      </div>
      <div class="col-10 q-py-md" v-if="!loading">
        <div class="row justify-center q-gutter-sm">
          <TournamentComponent
            v-for="(tournament, index) in tournaments"
            :key="index"
            :tournamentName="tournament.name"
            :tournamentLoc="tournament.location"
            :tournamentTime="dateFormater(tournament.date)"
            :tournamentVenue="tournament.venue"
            :tournamentIsHappening="tournament.isHappen"
            @click="handleTournamentClick(tournament.name, tournament.id)"
          />
        </div>
      </div>
      <div class="col"></div>
    </div>
  </q-page>
</template>

<script setup lang="ts">
import TournamentComponent from 'components/TournamentComponent.vue'
import { ref, onMounted } from 'vue'
import type { Event } from '../../components/models'
import { dateFormater } from '../../helper/DateFormater'
import { useRouter } from 'vue-router'
import { useEventStore } from 'src/stores/event'
import api from 'src/services/api'

const router = useRouter()

const eventStore = useEventStore()

const loading = ref(true)

const tournaments = ref<Event[]>([])

const fetchEvents = async () => {
  try {
    const response = await api.get('/events')

    const data = await response.data
    tournaments.value = data
  } catch (error) {
    console.error('Error fetching players:', error)
  } finally {
    loading.value = false
  }
}

const handleTournamentClick = async (event: string, id: number) => {
  eventStore.setEventId(id)
  await router.push({ name: 'EventDetail', params: { eventName: event } })
}

onMounted(async () => {
  await fetchEvents()
})
</script>

<style lang="sass"></style>

<style lang="scss" scoped></style>
