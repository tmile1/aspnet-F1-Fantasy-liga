(function () {
  const pageEl = document.querySelector(".manage-drivers-page");
  if (!pageEl) return;

  const budgetLimit =
    parseFloat(pageEl.getAttribute("data-budget-limit")) || 0.0;

  let state = {
    selectedDrivers: [],
    usedBudget: 0.0,
  };

  const usedBudgetEl = document.getElementById("usedBudget");
  const remainingBudgetEl = document.getElementById("remainingBudget");
  const driverCountEl = document.getElementById("driverCount");
  const budgetProgressBarEl = document.getElementById("budgetProgressBar");
  const budgetProgressContainer = document.querySelector(".budget-progress");
  const driverMarketList = document.getElementById("driverMarketList");
  const selectedTeamList = document.getElementById("selectedTeamList");
  const emptyTeamHint = document.getElementById("emptyTeamHint");
  const selectedDriverIdsInput = document.getElementById("selectedDriverIds");

  const statusMsgContainer = document.createElement("div");
  statusMsgContainer.className = "status-message";
  statusMsgContainer.setAttribute("aria-live", "polite");
  pageEl.querySelector(".build-team-header").appendChild(statusMsgContainer);

  const seedSelectedDrivers = () => {
    const selectedCards = selectedTeamList.querySelectorAll(".driver-card");
    selectedCards.forEach((card) => {
      const driverId = card.getAttribute("data-driver-id");
      const price = parseFloat(card.getAttribute("data-price")) || 0;
      state.selectedDrivers.push({ id: driverId, price: price });
      state.usedBudget += price;
    });
  };

  const updateHiddenInput = () => {
    if (!selectedDriverIdsInput) return;
    const ids = state.selectedDrivers.map((driver) => driver.id).join(",");
    selectedDriverIdsInput.value = ids;
  };

  const updateUI = () => {
    const remaining = Math.max(budgetLimit - state.usedBudget, 0);
    usedBudgetEl.textContent = state.usedBudget.toFixed(1) + " M";
    remainingBudgetEl.textContent = remaining.toFixed(1) + " M";
    driverCountEl.textContent = state.selectedDrivers.length.toString();

    const percentage =
      budgetLimit > 0 ? (state.usedBudget / budgetLimit) * 100 : 0;
    budgetProgressBarEl.style.width = Math.min(percentage, 100) + "%";
    budgetProgressContainer.setAttribute(
      "aria-valuenow",
      Math.min(percentage, 100).toFixed(0),
    );

    if (percentage >= 100) {
      budgetProgressBarEl.classList.add("over-budget");
    } else {
      budgetProgressBarEl.classList.remove("over-budget");
    }

    if (state.selectedDrivers.length === 0) {
      emptyTeamHint.style.display = "block";
    } else {
      emptyTeamHint.style.display = "none";
    }

    updateHiddenInput();
  };

  const showStatus = (msg) => {
    statusMsgContainer.textContent = msg;
  };

  const handleAddDriver = (card, btn) => {
    const driverId = card.getAttribute("data-driver-id");
    const price = parseFloat(card.getAttribute("data-price")) || 0;

    if (state.selectedDrivers.find((d) => d.id === driverId)) {
      return;
    }

    if (state.usedBudget + price > budgetLimit) {
      showStatus("Adding this driver would exceed the budget limit.");
      return;
    }

    state.selectedDrivers.push({ id: driverId, price: price });
    state.usedBudget += price;

    btn.disabled = true;
    btn.textContent = "Added";

    const clonedCard = card.cloneNode(true);
    const actionBtn = clonedCard.querySelector("button");
    actionBtn.className = "driver-action-btn remove-driver-btn";
    actionBtn.textContent = "Remove";
    actionBtn.disabled = false;

    selectedTeamList.appendChild(clonedCard);

    updateUI();
    showStatus("");
  };

  const handleRemoveDriver = (card) => {
    const driverId = card.getAttribute("data-driver-id");
    const price = parseFloat(card.getAttribute("data-price")) || 0;

    state.selectedDrivers = state.selectedDrivers.filter(
      (d) => d.id !== driverId,
    );
    state.usedBudget -= price;

    if (state.selectedDrivers.length === 0 || state.usedBudget < 0) {
      state.usedBudget = 0.0;
    }

    card.remove();

    const marketCard = driverMarketList.querySelector(
      `.driver-card[data-driver-id="${driverId}"]`,
    );
    if (marketCard) {
      const btn = marketCard.querySelector("button");
      btn.disabled = false;
      btn.textContent = "Add";
    }

    updateUI();
    showStatus("");
  };

  driverMarketList.addEventListener("click", function (e) {
    if (e.target.classList.contains("add-driver-btn")) {
      const card = e.target.closest(".driver-card");
      handleAddDriver(card, e.target);
    }
  });

  selectedTeamList.addEventListener("click", function (e) {
    if (e.target.classList.contains("remove-driver-btn")) {
      const card = e.target.closest(".driver-card");
      handleRemoveDriver(card);
    }
  });

  seedSelectedDrivers();
  updateUI();
})();
