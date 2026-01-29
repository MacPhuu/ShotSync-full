<template>
  <q-page class="row justify-center">
    <div class="col justify-center items-center">
      <div class="row justify-center items-center" v-if="loading" style="height: 60vh">
        <q-spinner-cube color="orange" size="5.5em" />
      </div>
    </div>
    <div class="col-12 q-py-md column"  v-if="!loading">
      <div
        class="event-infor-component w-100 bg-no-2 q-my-sm row"
        style="height: auto; border-radius: 20px; width: 100%"
      >
        <div class="col column justify-center items-center" style="width: 95%">
          <div
            class="row bg-primary text-white text-h6 justify-center"
            style="height: 40px; width: 30%; border-radius: 0 0 10px 10px"
          >
            BRANCHES
          </div>
          <div v-if="matchFound && groupedMatches.length > 0" class="column items-center" style="width: 100%;">
            <div v-for="(round, rIndex) in groupedMatches" :key="rIndex" class="column items-center w-100 q-mb-lg" style="width: 100%;">
                <div
                    class="row bg-primary text-white text-h6 justify-center q-my-md"
                    style="height: 40px; width: 60%; border-radius: 10px"
                >
                    {{ round.name }}
                </div>
                <div class="column q-my-sm items-center" style="width: 90%">
                    <BrancheComponent
                        class="bg-white text-primary q-mb-md"
                        v-for="(match, mIndex) in round.matches"
                        :key="mIndex"
                        :id="match.id"
                        :tableNumber="match.tableNumber || 'TBD'"
                        :firstPlayerName="match.firstPlayerName || 'TBD'"
                        :firstPlayerPoint="match.firstPlayerPoint"
                        :secondPlayerName="match.secondPlayerName || 'TBD'"
                        :secondPlayerPoint="match.secondPlayerPoint"
                        :isFinish="match.isFinish"
                    />
                </div>
            </div>
          </div>
          <div
            class="row q-my-lg justify-center text-white text-h4 text-italic"
            style="width: 95%"
            v-if="!matchFound || groupedMatches.length === 0"
          >
            No matches are currently being played or bracket not generated.
          </div>
        </div>
      </div>
    </div>
  </q-page>
</template>

<script setup lang="ts">
import BrancheComponent from 'components/BrancheComponent.vue'
import { ref, onMounted, computed } from 'vue'
import type { Match } from 'src/types/schema'
import { useEventStore } from 'src/stores/event'
import MatchService from 'src/services/match.service'

const eventStore = useEventStore()

const eventId = eventStore.getEventId

const loading = ref(true)

const matchFound = ref(true)

const matches = ref<Match[]>([])

const groupedMatches = computed(() => {
    const groups: { name: string; matches: Match[] }[] = [];
    const roundNames = [...new Set(matches.value.map(m => m.roundName || 'Unknown Round'))];

    // Custom sort order if needed, otherwise just appearance order
    roundNames.forEach(name => {
        groups.push({
            name: name,
            matches: matches.value.filter(m => m.roundName === name)
        });
    });

    return groups;
});

const fetchMatchs = async () => {
  try {
    const data = await MatchService.getMatchesByEvent(Number(eventId))
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
