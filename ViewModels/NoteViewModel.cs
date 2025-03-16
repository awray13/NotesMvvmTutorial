using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace Notes.ViewModels;

internal class NoteViewModel : ObservableObject, IQueryAttributable
{
    private Models.Note _note;

    public string Text
    {
        get => _note.Text;
        set
        {
            if (_note.Text != value)
            {
                _note.Text = value;
                OnPropertyChanged();
            }
        }
    }

    public DateTime Date => _note.Date;

    public string Identifier => _note.Filename;

    public ICommand SaveCommand { get; private set; }
    public ICommand DeleteCommand { get; private set; }

    public NoteViewModel()
    {
        _note = new Models.Note();
        SaveCommand = new AsyncRelayCommand(Save);
        DeleteCommand = new AsyncRelayCommand(Delete);
    }

    public NoteViewModel(Models.Note note)
    {
        _note = note;
        SaveCommand = new AsyncRelayCommand(Save);
        DeleteCommand = new AsyncRelayCommand(Delete);
    }

    // Save method to save the note
    private async Task Save()
    {
        _note.Date = DateTime.Now;
        _note.Save();
        await Shell.Current.GoToAsync($"..?saved={_note.Filename}");
    }

    // Delete method to delete the note
    private async Task Delete()
    {
        _note.Delete();
        await Shell.Current.GoToAsync("$..?deleted={_note.Filename}");
    }

    // Method to load the note from the query parameter
    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("load"))
        {
            _note = Models.Note.Load(query["load"].ToString());
            RefreshProperties();
        }
    }

    // Helper method to reload the note
    public void Reload()
    {
        _note = Models.Note.Load(_note.Filename);
        RefreshProperties();
    }

    // Helper method to refresh the properties
    private void RefreshProperties()
    {
        OnPropertyChanged(nameof(Text));
        OnPropertyChanged(nameof(Date));
    }
}
