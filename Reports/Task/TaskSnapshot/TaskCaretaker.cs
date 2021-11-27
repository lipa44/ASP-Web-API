using System;
using System.Collections.Generic;
using System.Linq;

namespace Reports.Task.TaskSnapshot
{
    public class TaskCaretaker
    {
        private readonly LinkedList<ITaskSnapshot> _snapshots;
        private readonly Task _task;

        public TaskCaretaker(Task task)
        {
            _task = task;
            _snapshots = new LinkedList<ITaskSnapshot>();
        }

        public void Backup()
        {
            _snapshots.AddLast(_task.MakeSnapshot());
        }

        public void Undo()
        {
            if (_snapshots.Count == 0) return;

            ITaskSnapshot snapshot = _snapshots.Last();
            _snapshots.Remove(snapshot);

            try
            {
                _task.RestoreSnapshot(snapshot);
            }
            catch (Exception)
            {
                Undo();
            }
        }
    }
}