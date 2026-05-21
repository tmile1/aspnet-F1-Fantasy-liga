// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

(() => {
  const isInteractiveTarget = (target) => {
    return !!target.closest("a, button, input, select, textarea, label");
  };

  const initAutocomplete = () => {
    const fields = document.querySelectorAll(".autocomplete-field");
    if (!fields.length) {
      return;
    }

    fields.forEach((field) => {
      const input = field.querySelector(".autocomplete-input");
      const hidden = field.querySelector("input[type='hidden']");
      const menu = field.querySelector(".autocomplete-menu");
      const searchUrl = field.dataset.autocompleteUrl;

      if (!input || !hidden || !menu || !searchUrl) {
        return;
      }

      let debounceTimer;

      const closeMenu = () => {
        menu.classList.remove("is-open");
        menu.innerHTML = "";
      };

      const openMenu = () => {
        if (menu.children.length > 0) {
          menu.classList.add("is-open");
        }
      };

      const renderResults = (items) => {
        menu.innerHTML = "";

        if (!items.length) {
          closeMenu();
          return;
        }

        items.forEach((item) => {
          const option = document.createElement("div");
          option.className = "autocomplete-item";
          option.textContent = item.label;
          option.dataset.id = item.id;
          option.dataset.label = item.label;
          menu.appendChild(option);
        });

        openMenu();
      };

      const runSearch = () => {
        const term = input.value.trim();
        const url = `${searchUrl}?term=${encodeURIComponent(term)}`;
        fetch(url, { headers: { "X-Requested-With": "XMLHttpRequest" } })
          .then((response) => response.json())
          .then((items) => {
            renderResults(items || []);
          })
          .catch(() => {
            closeMenu();
          });
      };

      input.addEventListener("input", () => {
        hidden.value = "";
        window.clearTimeout(debounceTimer);
        debounceTimer = window.setTimeout(runSearch, 250);
      });

      menu.addEventListener("click", (event) => {
        const option = event.target.closest(".autocomplete-item");
        if (!option) {
          return;
        }

        input.value = option.dataset.label || "";
        hidden.value = option.dataset.id || "";
        closeMenu();
      });

      document.addEventListener("click", (event) => {
        if (!field.contains(event.target)) {
          closeMenu();
        }
      });

      input.addEventListener("focus", () => {
        if (menu.children.length > 0) {
          openMenu();
        }
      });
    });
  };

  const initHomeCards = () => {
    const cards = document.querySelectorAll(".home-nav-grid .home-card");
    if (!cards.length) {
      return;
    }

    cards.forEach((card, index) => {
      window.setTimeout(() => {
        card.classList.add("is-visible");
      }, index * 90);
    });
  };

  const initPodium = () => {
    const blocks = document.querySelectorAll(".podium-block");
    if (!blocks.length) {
      return;
    }

    blocks.forEach((block, index) => {
      window.setTimeout(() => {
        block.classList.add("is-visible");
      }, index * 140);
    });
  };

  const initDatePickers = () => {
    const pickers = document.querySelectorAll("[data-date-picker]");
    if (!pickers.length) {
      return;
    }

    const language = (navigator.language || "").toLowerCase();
    const isCroatian = language.startsWith("hr");
    const weekStart = isCroatian ? 1 : 0;
    const monthLabels = isCroatian
      ? [
          "Sijecanj",
          "Veljaca",
          "Ozujak",
          "Travanj",
          "Svibanj",
          "Lipanj",
          "Srpanj",
          "Kolovoz",
          "Rujan",
          "Listopad",
          "Studeni",
          "Prosinac",
        ]
      : [
          "January",
          "February",
          "March",
          "April",
          "May",
          "June",
          "July",
          "August",
          "September",
          "October",
          "November",
          "December",
        ];
    const weekdayLabels = isCroatian
      ? ["Ne", "Po", "Ut", "Sr", "Ce", "Pe", "Su"]
      : ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];

    const pad = (value) => String(value).padStart(2, "0");

    const formatDisplayDate = (date) => {
      const day = pad(date.getDate());
      const month = pad(date.getMonth() + 1);
      const year = date.getFullYear();
      return isCroatian ? `${day}.${month}.${year}` : `${month}/${day}/${year}`;
    };

    const parseDisplayDate = (value) => {
      const trimmed = value.trim();
      if (!trimmed) {
        return null;
      }

      const hrMatch = /^([0-3]?\d)\.([0-1]?\d)\.(\d{4})$/.exec(trimmed);
      const enMatch = /^([0-1]?\d)\/([0-3]?\d)\/(\d{4})$/.exec(trimmed);
      const match = isCroatian ? hrMatch : enMatch;

      if (!match) {
        return null;
      }

      const first = Number(match[1]);
      const second = Number(match[2]);
      const year = Number(match[3]);
      const day = isCroatian ? first : second;
      const month = isCroatian ? second : first;

      if (month < 1 || month > 12 || day < 1 || day > 31) {
        return null;
      }

      const candidate = new Date(year, month - 1, day);
      if (
        candidate.getFullYear() !== year ||
        candidate.getMonth() !== month - 1 ||
        candidate.getDate() !== day
      ) {
        return null;
      }

      return candidate;
    };

    const parseIsoDate = (value) => {
      const match = /^(\d{4})-(\d{2})-(\d{2})$/.exec(value || "");
      if (!match) {
        return null;
      }

      const year = Number(match[1]);
      const month = Number(match[2]);
      const day = Number(match[3]);
      return new Date(year, month - 1, day);
    };

    const formatIsoDate = (date) => {
      return `${date.getFullYear()}-${pad(date.getMonth() + 1)}-${pad(
        date.getDate(),
      )}`;
    };

    const normalizeTime = (value) => {
      const match = /^([01]\d|2[0-3]):([0-5]\d)$/.exec(value || "");
      return match ? match[0] : null;
    };

    pickers.forEach((picker) => {
      const dateInput = picker.querySelector(".date-picker-input");
      const timeInput = picker.querySelector(".time-input");
      const hiddenInput = picker.querySelector("input[type='hidden']");
      const popover = picker.querySelector(".date-picker-popover");
      const title = picker.querySelector(".date-picker-title");
      const weekdays = picker.querySelector(".date-picker-weekdays");
      const grid = picker.querySelector(".date-picker-grid");
      const prevButton = picker.querySelector("[data-action='prev']");
      const nextButton = picker.querySelector("[data-action='next']");
      const showTime = picker.dataset.showTime === "true";
      const initialDate = picker.dataset.initialDate || "";
      const initialTime = picker.dataset.initialTime || "";

      if (
        !dateInput ||
        !hiddenInput ||
        !popover ||
        !title ||
        !weekdays ||
        !grid
      ) {
        return;
      }

      dateInput.placeholder = isCroatian ? "dd.MM.yyyy" : "MM/dd/yyyy";

      let selectedDate = parseIsoDate(initialDate);
      let visibleMonth = selectedDate
        ? new Date(selectedDate.getFullYear(), selectedDate.getMonth(), 1)
        : new Date();

      const updateHidden = () => {
        if (!hiddenInput) {
          return;
        }

        if (!selectedDate) {
          hiddenInput.value = "";
          return;
        }

        const isoDate = formatIsoDate(selectedDate);
        if (!showTime) {
          hiddenInput.value = isoDate;
          return;
        }

        const timeValue = timeInput ? normalizeTime(timeInput.value) : null;
        hiddenInput.value = `${isoDate}T${timeValue || "00:00"}`;
      };

      const closePopover = () => {
        popover.classList.remove("is-open");
        popover.setAttribute("aria-hidden", "true");
      };

      const openPopover = () => {
        popover.classList.add("is-open");
        popover.setAttribute("aria-hidden", "false");
        renderCalendar();
      };

      const renderWeekdays = () => {
        weekdays.innerHTML = "";
        for (let i = 0; i < 7; i += 1) {
          const labelIndex = (i + weekStart) % 7;
          const label = document.createElement("span");
          label.textContent = weekdayLabels[labelIndex];
          weekdays.appendChild(label);
        }
      };

      const renderCalendar = () => {
        const year = visibleMonth.getFullYear();
        const month = visibleMonth.getMonth();
        const firstDay = new Date(year, month, 1);
        const daysInMonth = new Date(year, month + 1, 0).getDate();
        const daysInPrevMonth = new Date(year, month, 0).getDate();
        const startOffset = (firstDay.getDay() - weekStart + 7) % 7;

        title.textContent = `${monthLabels[month]} ${year}`;
        grid.innerHTML = "";

        for (let i = 0; i < 42; i += 1) {
          const button = document.createElement("button");
          button.type = "button";
          button.className = "date-picker-day";

          let cellDay = 0;
          let cellMonth = month;
          let cellYear = year;

          if (i < startOffset) {
            cellDay = daysInPrevMonth - startOffset + i + 1;
            cellMonth = month - 1;
            if (cellMonth < 0) {
              cellMonth = 11;
              cellYear -= 1;
            }
            button.classList.add("is-muted");
          } else if (i >= startOffset + daysInMonth) {
            cellDay = i - (startOffset + daysInMonth) + 1;
            cellMonth = month + 1;
            if (cellMonth > 11) {
              cellMonth = 0;
              cellYear += 1;
            }
            button.classList.add("is-muted");
          } else {
            cellDay = i - startOffset + 1;
          }

          button.textContent = cellDay;
          button.dataset.year = cellYear;
          button.dataset.month = cellMonth;
          button.dataset.day = cellDay;

          if (
            selectedDate &&
            selectedDate.getFullYear() === cellYear &&
            selectedDate.getMonth() === cellMonth &&
            selectedDate.getDate() === cellDay
          ) {
            button.classList.add("is-selected");
          }

          grid.appendChild(button);
        }
      };

      if (selectedDate) {
        dateInput.value = formatDisplayDate(selectedDate);
      }

      if (timeInput) {
        timeInput.value =
          normalizeTime(initialTime) || (showTime ? "00:00" : "");
      }

      updateHidden();
      renderWeekdays();

      dateInput.addEventListener("focus", openPopover);
      dateInput.addEventListener("click", openPopover);

      const toggle = picker.querySelector(".date-picker-toggle");
      if (toggle) {
        toggle.addEventListener("click", () => {
          if (popover.classList.contains("is-open")) {
            closePopover();
          } else {
            openPopover();
          }
        });
      }

      if (prevButton) {
        prevButton.addEventListener("click", () => {
          visibleMonth = new Date(
            visibleMonth.getFullYear(),
            visibleMonth.getMonth() - 1,
            1,
          );
          renderCalendar();
        });
      }

      if (nextButton) {
        nextButton.addEventListener("click", () => {
          visibleMonth = new Date(
            visibleMonth.getFullYear(),
            visibleMonth.getMonth() + 1,
            1,
          );
          renderCalendar();
        });
      }

      grid.addEventListener("click", (event) => {
        const dayButton = event.target.closest(".date-picker-day");
        if (!dayButton) {
          return;
        }

        const year = Number(dayButton.dataset.year);
        const month = Number(dayButton.dataset.month);
        const day = Number(dayButton.dataset.day);
        selectedDate = new Date(year, month, day);
        visibleMonth = new Date(year, month, 1);
        dateInput.value = formatDisplayDate(selectedDate);
        updateHidden();
        renderCalendar();
        closePopover();
      });

      dateInput.addEventListener("blur", () => {
        const parsed = parseDisplayDate(dateInput.value);
        if (parsed) {
          selectedDate = parsed;
          visibleMonth = new Date(parsed.getFullYear(), parsed.getMonth(), 1);
          dateInput.value = formatDisplayDate(parsed);
        } else {
          selectedDate = null;
        }
        updateHidden();
        renderCalendar();
      });

      if (timeInput) {
        timeInput.addEventListener("input", () => {
          updateHidden();
        });
      }

      document.addEventListener("click", (event) => {
        if (!picker.contains(event.target)) {
          closePopover();
        }
      });
    });
  };

  const initDateDisplays = () => {
    const displays = document.querySelectorAll("[data-date-display]");
    if (!displays.length) {
      return;
    }

    const language = (navigator.language || "").toLowerCase();
    const isCroatian = language.startsWith("hr");
    const pad = (value) => String(value).padStart(2, "0");

    const formatDisplayDate = (date) => {
      const day = pad(date.getDate());
      const month = pad(date.getMonth() + 1);
      const year = date.getFullYear();
      return isCroatian ? `${day}.${month}.${year}` : `${month}/${day}/${year}`;
    };

    displays.forEach((display) => {
      const isoDate = display.dataset.iso || "";
      const match = /^(\d{4})-(\d{2})-(\d{2})$/.exec(isoDate);
      if (!match) {
        return;
      }

      const year = Number(match[1]);
      const month = Number(match[2]);
      const day = Number(match[3]);
      const date = new Date(year, month - 1, day);
      if (
        date.getFullYear() !== year ||
        date.getMonth() !== month - 1 ||
        date.getDate() !== day
      ) {
        return;
      }

      display.textContent = formatDisplayDate(date);
    });
  };

  window.refreshDateDisplays = initDateDisplays;

  const deleteModal = document.getElementById("delete-modal");
  const deleteForm = document.getElementById("delete-form");
  const deleteName = document.getElementById("delete-entity-name");

  const openDeleteModal = (trigger) => {
    if (!deleteModal || !deleteForm || !deleteName) {
      return;
    }

    deleteForm.setAttribute("action", trigger.dataset.deleteUrl || "");
    deleteName.textContent = trigger.dataset.entityName || "this item";
    deleteModal.classList.add("is-visible");
    deleteModal.setAttribute("aria-hidden", "false");
  };

  const closeDeleteModal = () => {
    if (!deleteModal) {
      return;
    }

    deleteModal.classList.remove("is-visible");
    deleteModal.setAttribute("aria-hidden", "true");
  };

  document.addEventListener("click", (event) => {
    const deleteTrigger = event.target.closest(".delete-trigger");
    if (deleteTrigger) {
      event.preventDefault();
      openDeleteModal(deleteTrigger);
      return;
    }

    if (
      event.target.matches("[data-modal-close]") ||
      event.target === deleteModal
    ) {
      event.preventDefault();
      closeDeleteModal();
      return;
    }

    const row = event.target.closest(".row-link[data-href]");
    if (!row || isInteractiveTarget(event.target)) {
      return;
    }

    window.location.href = row.dataset.href;
  });

  document.addEventListener("keydown", (event) => {
    if (event.key === "Escape") {
      closeDeleteModal();
      return;
    }

    const row = event.target.closest(".row-link[data-href]");
    if (!row || (event.key !== "Enter" && event.key !== " ")) {
      return;
    }

    event.preventDefault();
    window.location.href = row.dataset.href;
  });

  document.addEventListener("DOMContentLoaded", () => {
    initAutocomplete();
    initHomeCards();
    initPodium();
    initDatePickers();
    initDateDisplays();
  });
})();
