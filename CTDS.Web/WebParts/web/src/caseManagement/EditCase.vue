<template>
  <div>
    <appNavbar></appNavbar>
    <b-card no-body>
      <div class="mainCaseHeader">
        <b-card-header
          header-tag="header"
          header-text-variant="white"
          class="p-1"
        >
          <appEditCaseHeader></appEditCaseHeader>
        </b-card-header>
      </div>
      <b-tabs card class="myMainBodyCard">
        <b-tab title="Header" active>
          <appCaseManagementBody></appCaseManagementBody>
        </b-tab>
      </b-tabs>
    </b-card>
  </div>
</template>

<script>
import CaseManagementBody from "./CaseManagementBody.vue";
import EditCaseHeader from "./EditCaseHeader.vue";
import Navbar from "./../commonComponent/Navigationbar";

export default {
  components: {
    appEditCaseHeader: EditCaseHeader,
    appCaseManagementBody: CaseManagementBody,
    appNavbar: Navbar
  }
};
</script>

<style scoped>
@import url(./styles/caseBody.css);
</style>
