using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Notes.ViewModels;

internal class NotesViewModel : IQueryAttributable
{
    public ObservableCollection<ViewModels.NoteViewModel> AllNotes { get; }
    public ICommand NewCommand { get; }
    public ICommand SelectNoteCommand { get; }

    // Parameterless constructor which initializes the commands and loads the notes from the model
    public NotesViewModel()
    {
        AllNotes = new ObservableCollection<ViewModels.NoteViewModel>(Models.Note.LoadAll().Select(n => new NoteViewModel(n)));
        NewCommand = new AsyncRelayCommand(NewNoteAsync);
        SelectNoteCommand = new AsyncRelayCommand<ViewModels.NoteViewModel>(SelectNoteAsync);
    }

    // Method targeting the NewCommand
    private async Task NewNoteAsync()
    {
        await Shell.Current.GoToAsync(nameof(Views.NotePage));
    }

    // Method targeting the SelectNoteCommand
    private async Task SelectNoteAsync(ViewModels.NoteViewModel note)
    {
        await Shell.Current.GoToAsync("${nameof(Views.NotePage)}?load={note.Identifier}");
    }
}
