<template>
  <q-page class="row q-pt-lg justify-evenly">
    <div class="row items-center justify-center" style="width: 100%" v-if="loading">
      <q-spinner-cube color="orange" size="5.5em" />
    </div>
    <div class="col-1" style="max-width: 100px"></div>
    <div class="col" v-if="!loading">
      <div style="width: 23%">
        <q-card flat class="q-ma-sm" style="background-color: rgba(0, 0, 0, 0)">
          <div class="row items-center" style="height: 150px">
            <q-btn round class="q-pa-md" color="secondary" icon="add" @click="createTournament">
              <q-tooltip
                class="bg-primary"
                anchor="center right"
                self="center start"
                style="font-size: 14px"
                >Create Tournament</q-tooltip
              >
            </q-btn>
          </div>
        </q-card>
      </div>
      <div class="row q-gutter-sm">
        <EventComponent
          v-for="(event, index) in events"
          :key="index"
          :eventName="event.name"
          :eventLoc="event.location"
          :eventTime="dateFormater(event.date)"
          :eventVenue="event.venue"
          :eventIsHappening="event.isHappen"
          @click="handleEventClick(event.name, event.id)"
        />
      </div>
    </div>
  </q-page>
</template>

<script setup lang="ts">
import EventComponent from 'components/HostEventComponent.vue'
import { ref, onMounted } from 'vue'
import type { Event as ComponentEvent } from 'components/models'
import type { Event as SchemaEvent } from 'src/types/schema'
import { dateFormater } from 'src/helper/DateFormater'
import { useRouter } from 'vue-router'
import { useEventStore } from 'src/stores/event'
import EventService from 'src/services/event.service'
import ROUTE_PATHS from 'src/router/route-paths'

const router = useRouter()

const eventStore = useEventStore()

const loading = ref(true)

const events = ref<ComponentEvent[]>([])

const fetchEvents = async () => {
  try {
    const userId = localStorage.getItem('userId') || sessionStorage.getItem('userId');
    if (!userId) {
        console.error("No user ID found, redirecting to login");
        await router.push(ROUTE_PATHS.AUTH.LOGIN);
        return;
    }

    // Call the new API service method
    const data: SchemaEvent[] = await EventService.getEventsByHost(Number(userId));

    // Map SchemaEvent to ComponentEvent
    events.value = data.map((e) : ComponentEvent => ({
        id: e.id,
        name: e.name || 'Unnamed Event',
        venue: e.venue || 'Unknown Venue',
        location: e.location || 'Unknown Location',
        date: e.date,
        isHappen: e.isHappen
    }));
  } catch (error) {
    console.error('Error fetching events:', error)
  } finally {
    loading.value = false
  }
}

const handleEventClick = async (event: string, id: number) => {
  eventStore.setEventId(id)
  await router.push({ name: 'HostEventDetail', params: { eventName: event } })
}

const createTournament = async () => {
  await router.push('create_event')
}

onMounted(async () => {
  await fetchEvents()
})
</script>

<style lang="sass" scoped></style>
